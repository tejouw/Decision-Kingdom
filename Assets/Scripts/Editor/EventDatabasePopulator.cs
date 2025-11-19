#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DecisionKingdom.Data;
using DecisionKingdom.Content;

namespace DecisionKingdom.Editor
{
    /// <summary>
    /// Event database'ini içerikle doldurmak için editor aracı
    /// </summary>
    public class EventDatabasePopulator : EditorWindow
    {
        private EventDatabase _targetDatabase;
        private Vector2 _scrollPosition;
        private bool _showCharacters = true;
        private bool _showEvents = true;

        [MenuItem("Decision Kingdom/Event Database Populator")]
        public static void ShowWindow()
        {
            var window = GetWindow<EventDatabasePopulator>("Event Database Populator");
            window.minSize = new Vector2(400, 500);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Event Database Populator", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            _targetDatabase = (EventDatabase)EditorGUILayout.ObjectField(
                "Target Database",
                _targetDatabase,
                typeof(EventDatabase),
                false
            );

            if (_targetDatabase == null)
            {
                EditorGUILayout.HelpBox(
                    "EventDatabase asset seçin veya oluşturun.\n\n" +
                    "Oluşturmak için: Assets > Create > Decision Kingdom > Event Database",
                    MessageType.Info
                );

                if (GUILayout.Button("Yeni Database Oluştur"))
                {
                    CreateNewDatabase();
                }
                return;
            }

            EditorGUILayout.Space(10);

            // Mevcut içerik bilgisi
            EditorGUILayout.LabelField("Mevcut İçerik", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Karakterler: {_targetDatabase.CharacterCount}");
            EditorGUILayout.LabelField($"Eventler: {_targetDatabase.EventCount}");

            EditorGUILayout.Space(10);

            // İçerik önizleme
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(200));

            _showCharacters = EditorGUILayout.Foldout(_showCharacters, "Medieval Karakterler (6)");
            if (_showCharacters)
            {
                EditorGUI.indentLevel++;
                var characters = MedievalContent.GetCharacters();
                foreach (var character in characters)
                {
                    EditorGUILayout.LabelField($"- {character.FullName}: {character.description}");
                }
                EditorGUI.indentLevel--;
            }

            _showEvents = EditorGUILayout.Foldout(_showEvents, $"Medieval Eventler ({MedievalContent.GetEvents().Count})");
            if (_showEvents)
            {
                EditorGUI.indentLevel++;
                var events = MedievalContent.GetEvents();
                int count = 0;
                foreach (var evt in events)
                {
                    if (count < 10)
                    {
                        EditorGUILayout.LabelField($"- {evt.id}: {evt.description}");
                    }
                    count++;
                }
                if (count > 10)
                {
                    EditorGUILayout.LabelField($"... ve {count - 10} event daha");
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);

            // Aksiyon butonları
            EditorGUILayout.LabelField("Aksiyonlar", EditorStyles.boldLabel);

            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Medieval İçeriği Ekle (Mevcut Üzerine)", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog(
                    "İçerik Ekleme",
                    "Medieval karakterler ve eventler mevcut database'e eklenecek. Devam edilsin mi?",
                    "Evet",
                    "İptal"))
                {
                    AddMedievalContent();
                }
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Database'i Temizle ve Yeniden Doldur", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog(
                    "Uyarı",
                    "Tüm mevcut içerik silinecek ve Medieval içeriğiyle değiştirilecek. Devam edilsin mi?",
                    "Evet, Temizle ve Doldur",
                    "İptal"))
                {
                    ClearAndPopulateDatabase();
                }
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Database'i Doğrula"))
            {
                _targetDatabase.ValidateDatabase();
            }
        }

        private void CreateNewDatabase()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Event Database Oluştur",
                "EventDatabase",
                "asset",
                "Event Database asset'ını kaydedin"
            );

            if (string.IsNullOrEmpty(path))
                return;

            var database = ScriptableObject.CreateInstance<EventDatabase>();
            AssetDatabase.CreateAsset(database, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _targetDatabase = database;
            EditorGUIUtility.PingObject(database);

            Debug.Log($"[EventDatabasePopulator] Yeni database oluşturuldu: {path}");
        }

        private void AddMedievalContent()
        {
            // Karakterleri ekle
            var characters = MedievalContent.GetCharacters();
            foreach (var character in characters)
            {
                _targetDatabase.AddCharacter(character);
            }

            // Eventleri ekle
            var events = MedievalContent.GetEvents();
            foreach (var evt in events)
            {
                _targetDatabase.AddEvent(evt);
            }

            EditorUtility.SetDirty(_targetDatabase);
            AssetDatabase.SaveAssets();

            Debug.Log($"[EventDatabasePopulator] {characters.Count} karakter ve {events.Count} event eklendi.");
            EditorUtility.DisplayDialog(
                "Başarılı",
                $"{characters.Count} karakter ve {events.Count} event database'e eklendi.",
                "Tamam"
            );
        }

        private void ClearAndPopulateDatabase()
        {
            // Database'i temizle
            var serializedObject = new SerializedObject(_targetDatabase);
            var eventsProperty = serializedObject.FindProperty("_events");
            var charactersProperty = serializedObject.FindProperty("_characters");

            eventsProperty.ClearArray();
            charactersProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();

            // Yeniden doldur
            AddMedievalContent();

            Debug.Log("[EventDatabasePopulator] Database temizlendi ve yeniden dolduruldu.");
        }
    }
}
#endif
