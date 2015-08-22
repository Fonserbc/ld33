using UnityEngine;
using System.Collections;

public class PlayerMesh : MonoBehaviour {

	const float VERTEX_DISTANCE = 0.1f;
	const int MAX_CONTACTS = 4;
	const float LERP_TIME = 0.7f;
	const float MIN_ATRACTION = 0.2f;

	public Sprite sprite;

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	// Mesh
	int vertexCount = 0;
	int triangleCount = 0;

	Vector3[] vertices;
	Vector3[] verticesStartPos;
	Color[] colors;
	Vector2[] texCoords;
	int[] triangles;

	Mesh spriteMesh;

	// Collider
	ContactPoint2D[] contacts = new ContactPoint2D[MAX_CONTACTS];
	int contactsCount = 0;
	float[] verticesStickyness;
	float lerpingTime = 0f;
	int lastContactsCount = 0;

	// Use this for initialization
	void Start () {
		InitMesh();
		InitStickyness();
	}

	void InitMesh() {
		/*	InitMesh();*/
		float width = sprite.bounds.size.x;
		float height = sprite.bounds.size.y;

		int vertexCountX = Mathf.CeilToInt(width/VERTEX_DISTANCE);
		int vertexCountY = Mathf.CeilToInt(height/VERTEX_DISTANCE);
		vertexCount = vertexCountX * vertexCountY;

		vertices = new Vector3[vertexCount];
		verticesStartPos = new Vector3[vertexCount];
		colors = new Color[vertexCount];
		texCoords = new Vector2[vertexCount];
		triangles = new int[vertexCount * 6];

		int index = 0;
		Vector3 vPos = Vector3.zero;
		Vector2 uv = Vector2.zero;
		for(int i = 0; i < vertexCountX; i++) {
			for(int j = 0; j < vertexCountY; j++) {
				index = (i*vertexCountY) + j;

				vPos.x = (-width * 0.5f) + ((width/((float)vertexCountX - 1)) * i);
				vPos.y = (-height * 0.5f) + ((height/((float)vertexCountY - 1)) * j);
				vertices[index] = vPos;
				verticesStartPos[index] = vPos;

				colors[index] = Color.white;

				uv.x = (float)i/(float)(vertexCountX - 1);
				uv.y = (float)j/(float)(vertexCountY - 1);
				texCoords[index] = uv;
			}
		}

		triangleCount = 0;
		for(int i = 0; i < vertexCountX - 1; i++) {
			for(int j = 0; j < vertexCountY - 1; j++) {
				int p0 = (i*vertexCountY) + j;
				int p1 = (i*vertexCountY) + (j + 1);
				int p2 = ((i + 1)*vertexCountY) + (j + 1);
				int p3 = ((i + 1)*vertexCountY) + j;
				
				triangles[triangleCount++] = p0;
				triangles[triangleCount++] = p1;
				triangles[triangleCount++] = p2;
				
				triangles[triangleCount++] = p3;
				triangles[triangleCount++] = p0;
				triangles[triangleCount++] = p2;
			}
		}

		Vector2 minTextureCoords = Vector2.zero;
		Vector2 maxTextureCoords = Vector2.zero;

		GetMinMaxTextureRect(out minTextureCoords, out maxTextureCoords);
		FixTextureCoordinates(minTextureCoords, maxTextureCoords);

		// Material
		Material material = null;

		material = new Material(Shader.Find("Sprites/Default"));
		material.SetTexture(0, sprite.texture);
		material.name = sprite.texture.name;
		
		meshRenderer.sharedMaterial = material;

		//Mesh
		spriteMesh = new Mesh();
		spriteMesh.name = "Sticky Mesh";
		spriteMesh.MarkDynamic();
		meshFilter.mesh = spriteMesh;
		
		spriteMesh.Clear();
		spriteMesh.vertices = vertices;
		spriteMesh.uv = texCoords;
		spriteMesh.triangles = triangles;
		spriteMesh.colors = colors;
		spriteMesh.RecalculateBounds();
		spriteMesh.RecalculateNormals();
	}

	void InitStickyness() {
		verticesStickyness = new float[vertexCount];
	}

	void GetMinMaxTextureRect(out Vector2 min, out Vector2 max)
	{
		Rect textureRect = sprite.textureRect;
		min = new Vector2(textureRect.xMin/(float)sprite.texture.width, textureRect.yMin/(float)sprite.texture.height);
		max = new Vector2(textureRect.xMax/(float)sprite.texture.width, textureRect.yMax/(float)sprite.texture.height);
	}

	void FixTextureCoordinates(Vector2 min, Vector2 max)
	{
		Vector2 offset;
		for(int i = 0; i < vertexCount; i++)
		{
			offset = max - min;
			offset.Scale(texCoords[i]);
			texCoords[i] = min + offset;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if (lastContactsCount != contactsCount) {
			lastContactsCount = contactsCount;
			lerpingTime = 0f;
		}

		if (lerpingTime < LERP_TIME) {
			lerpingTime += Time.fixedDeltaTime;

			if (lerpingTime > LERP_TIME) {
				lerpingTime = LERP_TIME;
			}

			Vector3 totalOffset = Vector3.zero;
			Vector3 deltaPos = Vector3.zero;
			Vector3 worldPos = Vector3.zero;
			float angle = 0f;
			float tan = 0f;
			
			float distanceToPlane = 0f;
			float distanceFactor = 0f;
			for(int i = 0; i < vertexCount; i++)
			{
				totalOffset = Vector3.zero;
				for(int k = 0; k < contactsCount; k++)
				{
					worldPos = transform.position + verticesStartPos[i];
					deltaPos = worldPos - (Vector3)contacts[k].point;
					angle = Vector2.Angle(contacts[k].normal, deltaPos);
					tan = Mathf.Tan(angle*Mathf.Deg2Rad);
					
					distanceToPlane = Mathf.Sqrt(deltaPos.sqrMagnitude/(1f + tan*tan));
					if (angle < 90f) {
						distanceToPlane = -distanceToPlane;
					}

					distanceFactor = 1f - Mathf.Min(1f, Mathf.Abs(distanceToPlane)/sprite.bounds.size.x);

					distanceFactor = MIN_ATRACTION + (1f - MIN_ATRACTION)*distanceFactor;

					totalOffset += distanceFactor*distanceToPlane*(Vector3)contacts[k].normal;
				}
				if (contactsCount > 0) totalOffset /= (float)contactsCount;
				
				vertices[i] = Vector3.Lerp(vertices[i], verticesStartPos[i] + totalOffset, Easing.Cubic.Out(lerpingTime/LERP_TIME));
			}
			
			// Update the mesh
			spriteMesh.vertices = vertices;
			spriteMesh.RecalculateBounds();
			spriteMesh.RecalculateNormals();
		}

		contactsCount = 0;
	}

	void OnCollisionStay2D(Collision2D col) {
		for (int i = 0; i < col.contacts.Length && contactsCount < MAX_CONTACTS; ++i) {
			contacts[contactsCount++] = col.contacts[i];
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
	}

	void OnCollisionExit2D(Collision2D col) {
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;

		//Gizmos.DrawSphere(transform.position + sprite.bounds.center, sprite.bounds.extents.x);
		
		Color wantedColor;
		if (verticesStartPos != null && verticesStartPos.Length > 0 && false) {
			for (int k = 0; k < contactsCount; ++k) {
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(contacts[k].point, 0.07f);
			}

			for (int i = 0; i < vertexCount; ++i) {
				//Gizmos.color = Color.blue;
				//Gizmos.DrawWireSphere(verticesStartPos[i] + transform.position, 0.07f);
				
				
				for (int k = 0; k < contactsCount; ++k) {
					Vector3 deltaPos = transform.position + verticesStartPos[i] - (Vector3)contacts[k].point;
					float angle = Vector2.Angle(contacts[k].normal, deltaPos);
					float tan = Mathf.Tan(angle*Mathf.Deg2Rad);
					
					float distanceToPlane = float.IsNaN(tan)? 0f : Mathf.Sqrt(deltaPos.sqrMagnitude/(1f + tan*tan));
					
					wantedColor = Color.red;
					if (angle < 90f) {
						distanceToPlane = -distanceToPlane;
						wantedColor = Color.green;
					}

					float distanceFactor = 1f - Mathf.Min(1f, Mathf.Abs(distanceToPlane)/sprite.bounds.size.x);

					wantedColor *= distanceFactor;
					//Debug.Log("Angle "+angle+ ", Tan " +tan+", distanceToPlane "+distanceToPlane);

					Gizmos.color = wantedColor;
					Gizmos.DrawLine(verticesStartPos[i] + transform.position + new Vector3(i*0.001f, 0, 0), verticesStartPos[i] + transform.position + new Vector3(i*0.001f, 0, 0) + ((Vector3)contacts[k].normal)*distanceToPlane);
				}
			}

		}
	}
}
