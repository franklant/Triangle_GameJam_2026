using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Added for scene loading

public class GameManager : MonoBehaviour
{
	[Header("Game Elements")]
	[Range(2, 6)]
	[SerializeField] private int difficulty = 3;
	[SerializeField] private Transform gameHolder;
	[SerializeField] private Transform piecePrefab;

	[Header("UI Element")]
	[SerializeField] private List<Texture2D> imageTextures;
	[SerializeField] private Transform puzzleSelectPanel;
	[SerializeField] private Image puzzleSelectPrefab;

	[Header("Transition Settings")] // Added transition settings
	public string nextAreaName = "mind";
	public int targetSpawnID;
	public CanvasGroup fadeCanvasGroup;

	private List<Transform> pieces;
	private Vector2Int dimensions;
	private float width;
	private float height;

	private Transform draggingPiece = null;
	private Vector3 offset;

	private int piecesCorrect;

	private void Start()
	{
		// Makes the cursor appear
		Cursor.visible = true;

		// Unlocks the cursor so it can move freely around the screen
		Cursor.lockState = CursorLockMode.None;

		if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0; // Initialize fade

		foreach (Texture2D texture in imageTextures)
		{
			Image image = Instantiate(puzzleSelectPrefab, puzzleSelectPanel);
			image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			image.GetComponent<Button>().onClick.AddListener(delegate { StartGame(texture); });
		}
	}

	public void StartGame(Texture2D jigsawTexture)
	{
		puzzleSelectPanel.gameObject.SetActive(false);
		pieces = new List<Transform>();
		dimensions = GetDimensions(jigsawTexture, difficulty);
		CreateJigsawPieces(jigsawTexture);
		Scatter();
		UpdateBorder();
		piecesCorrect = 0;
	}

	// ... (GetDimensions and CreateJigsawPieces logic remains the same)

	Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
	{
		Vector2Int dimensions = Vector2Int.zero;
		if (jigsawTexture.width < jigsawTexture.height)
		{
			dimensions.x = difficulty;
			dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
		}
		else
		{
			dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height; ;
			dimensions.y = difficulty;
		}
		return dimensions;
	}

	void CreateJigsawPieces(Texture2D jigsawTexture)
	{
		height = 1f / dimensions.y;
		width = 1f / dimensions.x;

		for (int row = 0; row < dimensions.y; row++)
		{
			for (int col = 0; col < dimensions.x; col++)
			{
				Transform piece = Instantiate(piecePrefab, gameHolder);
				float xPos = (col - (dimensions.x - 1) / 2f) * width;
				float yPos = (row - (dimensions.y - 1) / 2f) * height;

				piece.localPosition = new Vector3(xPos, yPos, -1);
				piece.localScale = new Vector3(width, height, 1f);
				piece.name = $"Piece {(row * dimensions.x) + col}";
				pieces.Add(piece);

				Vector2[] uv = new Vector2[4];
				uv[0] = new Vector2(width * col, height * row);
				uv[1] = new Vector2(width * (col + 1), height * row);
				uv[2] = new Vector2(width * col, height * (row + 1));
				uv[3] = new Vector2(width * (col + 1), height * (row + 1));

				Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
				mesh.uv = uv;
				piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
			}
		}
	}

	private void Scatter()
	{
		float orthoHeight = Camera.main.orthographicSize;
		float screenAspect = (float)Screen.width / Screen.height;
		float orthoWidth = (screenAspect * orthoHeight);

		float pieceWidth = width * gameHolder.localScale.x;
		float pieceHeight = height * gameHolder.localScale.y;

		orthoHeight -= pieceHeight;
		orthoWidth -= pieceWidth;

		foreach (Transform piece in pieces)
		{
			float x = Random.Range(-orthoWidth, orthoWidth);
			float y = Random.Range(-orthoHeight, orthoHeight);
			piece.position = new Vector3(x, y, -1);
		}
	}

	public void UpdateBorder()
	{
		LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();
		float halfWidth = (width * dimensions.x) / 2f;
		float halfHeight = (height * dimensions.y) / 2f;

		lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, 0));
		lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, 0));
		lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, 0));
		lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, 0));
		lineRenderer.enabled = true;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit)
			{
				draggingPiece = hit.transform;
				offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
				offset += Vector3.back;
			}
		}
		if (draggingPiece && Input.GetMouseButtonUp(0))
		{
			SnapAndDisableIfCorrect();
			draggingPiece.position += Vector3.forward;
			draggingPiece = null;
		}

		if (draggingPiece)
		{
			Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			newPosition += offset;
			draggingPiece.position = newPosition;
		}
	}

	private void SnapAndDisableIfCorrect()
	{
		int pieceIndex = pieces.IndexOf(draggingPiece);
		int col = pieceIndex % dimensions.x;
		int row = pieceIndex / dimensions.x; // Fixed row calculation
		Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2), (-height * dimensions.y / 2) + (height * row) + (height / 2));

		if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2))
		{
			draggingPiece.localPosition = targetPosition;
			draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
			piecesCorrect++;
			if (piecesCorrect == pieces.Count)
			{
				StartCoroutine(EndGameWithTransition()); // Changed to transition
			}
		}
	}

	IEnumerator EndGameWithTransition()
	{
		yield return new WaitForSeconds(1.0f);

		// Tell teleport system where to go
		sceneTeleportator.nextSpawnPoint = targetSpawnID;

		if (fadeCanvasGroup != null)
		{
			float elapsed = 0;
			while (elapsed < 1.0f)
			{
				elapsed += Time.deltaTime;
				fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / 1.0f);
				yield return null;
			}
		}

		if (!string.IsNullOrEmpty(nextAreaName))
		{
			SceneManager.LoadScene(nextAreaName);
		}
	}

	public void EndGame()
	{
		foreach (Transform piece in pieces) Destroy(piece.gameObject);
		pieces.Clear();
		gameHolder.GetComponent<LineRenderer>().enabled = false;
		puzzleSelectPanel.gameObject.SetActive(true);
	}
}
