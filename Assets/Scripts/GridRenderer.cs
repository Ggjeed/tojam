﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GridRenderer : MonoBehaviour {

	public TextAsset LevelFile;

	private int m_zoom = 1;
	private const int kGridWidth = 16;
	private const int kGridHeight = 8;
	private const int kSquarePixelSize = 64;

	struct SquareInfo
	{
		public int id;
		public string name;
	}

	Dictionary<char, SquareInfo> m_info;
	int m_nextId = 1;

	GameObject[] m_prefabs;

	int[,] m_map;

	GameObject createGridSquare(float x, float y, string assetName)
	{
		return null;
	}

	void LoadLevel()
	{
		var file = LevelFile.text;
		var lines = file.Split ('\n');
		bool readingMap = true;
		m_info = new Dictionary<char, SquareInfo> ();
		m_map = new int[kGridHeight, kGridWidth];
		int y = 0;

		foreach (string line in lines) 
		{
			if (readingMap)
			{
				if (line.Length == 0)
				{
					readingMap = false;
					continue;
				}
				else if (line.Length == kGridWidth)
				{
					// Processing a line of the map
					for (int i = 0; i < kGridWidth; ++i)
					{
						if (line[i] == '.')
						{
							// Gap in the grid
							m_map[y, i] = 0;
						}
						else
						{
							int id = 0;

							// Grab the id for the square
							if (m_info.ContainsKey(line[i]))
							{
								id = m_info[line[i]].id;
							}
							else
							{
								SquareInfo info = new SquareInfo();
								info.id = m_nextId++;
								m_info.Add(line[i], info);
								id = info.id;
							}
							m_map[y, i] = id;
						}
					}
				}
				else
				{
					// Invalid line
				}
			}
			else
			{
				// We are reading the codes
				string[] codes = line.Trim().Split(' ');

				if (codes.Length == 2 && codes[0].Length == 1)
				{
					char ch = codes[0][0];
					if (m_info.ContainsKey(ch))
					{
						var info = m_info[ch];
						info.name = codes[1];
						m_info[ch] = info;
					}
				}
			} // readingMap?

			++y;
		} // foreach line

		// Create prefabs
		m_prefabs = new GameObject[m_nextId-1];
		foreach (var infoPair in m_info)
		{
			SquareInfo info = infoPair.Value;
			GameObject gob = new GameObject(string.Format("Prefab-Square{0}", info.id));
			gob.SetActive(false);
			m_prefabs[info.id - 1] = gob;

			SpriteRenderer renderer = gob.AddComponent<SpriteRenderer>();
			Texture2D tex = Resources.Load<Texture2D>(info.name);
			renderer.sprite = Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.0f, 1.0f));
			Debug.Log(renderer.sprite);
		}
	}

	void CreateSquare(int x, int y, int id, string name)
	{
		var pos = new Vector3((float)x, (float)y, 0);

		GameObject gob = GameObject.Instantiate(m_prefabs[id - 1]);
		gob.name = name;
		gob.SetActive(true);
		gob.transform.position = pos;
	}

	// Use this for initialization
	void Start () {
		int w = Screen.width;
		int h = Screen.height;

		m_zoom = System.Math.Min(w / (kSquarePixelSize * kGridWidth), h / (kSquarePixelSize * kGridHeight));

		// Calculate the top-left and bottom-right coordinate of the screen in world coordinates
		var bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
		var topRight = Camera.main.ViewportToWorldPoint(new Vector3 (1.0f, 1.0f, 0));

		LoadLevel();

		CreateSquare(0, 0, 1, "Test");

		/*
		for (int row = 0; row < kGridHeight; ++row)
		{
			for (int col = 0; col < kGridWidth; ++col)
			{
				int id = m_map[row, col];

				if (id != 0)
				{
					var pos = new Vector3(
						(float)(col * kSquarePixelSize),
						Screen.height - ((float)(row * kSquarePixelSize)),
						0.0f);
					var wpos = Camera.main.ScreenToWorldPoint(pos);
					wpos.z = 0.0f;

					GameObject gob = GameObject.Instantiate(m_prefabs[0]);
					gob.name = string.Format("Sq:{0:D2}-{1:D2}", row, col);
					gob.SetActive(true);
					gob.transform.position = pos;
				}
			}	
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}