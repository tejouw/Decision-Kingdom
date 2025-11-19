using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Basari sistemi - 100+ basari ile
    /// Faz 4: Meta Sistemler
    /// </summary>
    public class AchievementSystem : MonoBehaviour
    {
        public static AchievementSystem Instance { get; private set; }

        [Header("Achievement Data")]
        [SerializeField] private HashSet<string> _unlockedAchievements = new HashSet<string>();

        // Events
        public event Action<Achievement> OnAchievementUnlocked;
        public event Action<int> OnProgressUpdated;

        private static Dictionary<string, Achievement> _achievements;

        #region Properties
        public int UnlockedCount => _unlockedAchievements.Count;
        public int TotalCount => _achievements?.Count ?? 0;
        public float CompletionPercentage => TotalCount > 0 ? (float)UnlockedCount / TotalCount * 100f : 0f;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAchievements();
            LoadAchievements();
        }
        #endregion

        #region Achievement Initialization
        private void InitializeAchievements()
        {
            _achievements = new Dictionary<string, Achievement>();

            // === HAYATTA KALMA BASARILARI (15) ===
            AddAchievement("survive_10", "Yeni Baslayanlar", "10 tur hayatta kal", AchievementCategory.Survival, 10, false);
            AddAchievement("survive_25", "Dayanikli", "25 tur hayatta kal", AchievementCategory.Survival, 20, false);
            AddAchievement("survive_50", "Tecrubeli Hukumdar", "50 tur hayatta kal", AchievementCategory.Survival, 50, false);
            AddAchievement("survive_75", "Uzun Omurlu", "75 tur hayatta kal", AchievementCategory.Survival, 75, false);
            AddAchievement("survive_100", "Efsanevi Kral", "100 tur hayatta kal", AchievementCategory.Survival, 100, false);
            AddAchievement("survive_150", "Olumsuze Yakin", "150 tur hayatta kal", AchievementCategory.Survival, 150, false);
            AddAchievement("survive_200", "Zaman Ustasi", "200 tur hayatta kal", AchievementCategory.Survival, 200, false);
            AddAchievement("quick_death", "Hizli Son", "5 turdan once ol", AchievementCategory.Survival, 5, false);
            AddAchievement("first_victory", "Ilk Zafer", "Herhangi bir zafer sonu elde et", AchievementCategory.Survival, 30, false);
            AddAchievement("five_victories", "Zafer Koleksiyoncusu", "5 farkli zafer sonu elde et", AchievementCategory.Survival, 100, false);
            AddAchievement("ten_games", "Kararlı Oyuncu", "10 oyun tamamla", AchievementCategory.Survival, 25, false);
            AddAchievement("fifty_games", "Bagimsiz", "50 oyun tamamla", AchievementCategory.Survival, 100, false);
            AddAchievement("hundred_games", "Efsane", "100 oyun tamamla", AchievementCategory.Survival, 200, false);
            AddAchievement("no_death_30", "Dikkatli Yonetici", "30 turda hic kaynak 20'nin altina dusmeden", AchievementCategory.Survival, 60, false);
            AddAchievement("all_deaths", "Olum Koleksiyoncusu", "Her farkli sekilde ol", AchievementCategory.Survival, 150, true);

            // === KAYNAK BASARILARI (20) ===
            AddAchievement("gold_max", "Midas", "Altini 100'e ulastir", AchievementCategory.Resource, 25, false);
            AddAchievement("happiness_max", "Sevgili Kral", "Mutlulugu 100'e ulastir", AchievementCategory.Resource, 25, false);
            AddAchievement("military_max", "Savas Lordu", "Askeri gucu 100'e ulastir", AchievementCategory.Resource, 25, false);
            AddAchievement("faith_max", "Kutsal Hukumdar", "Inanci 100'e ulastir", AchievementCategory.Resource, 25, false);
            AddAchievement("gold_min", "Iflas Ettin", "Altin 0'a dust", AchievementCategory.Resource, 10, false);
            AddAchievement("happiness_min", "Devrildin", "Mutluluk 0'a dust", AchievementCategory.Resource, 10, false);
            AddAchievement("military_min", "Istila Edildin", "Askeri guc 0'a dust", AchievementCategory.Resource, 10, false);
            AddAchievement("faith_min", "Kaos", "Inanc 0'a dust", AchievementCategory.Resource, 10, false);
            AddAchievement("perfect_balance", "Mukemmel Denge", "Tum kaynaklari 45-55 arasinda tut (30 tur)", AchievementCategory.Resource, 100, false);
            AddAchievement("rich_king", "Zengin Kral", "Altin 80+ ile 50 tur hayatta kal", AchievementCategory.Resource, 75, false);
            AddAchievement("loved_king", "Sevilen Kral", "Mutluluk 80+ ile 50 tur hayatta kal", AchievementCategory.Resource, 75, false);
            AddAchievement("war_king", "Savasci Kral", "Askeri 80+ ile 50 tur hayatta kal", AchievementCategory.Resource, 75, false);
            AddAchievement("holy_king", "Dindar Kral", "Inanc 80+ ile 50 tur hayatta kal", AchievementCategory.Resource, 75, false);
            AddAchievement("balanced_30", "Dengeli Yonetici", "30 tur tum kaynaklar 30-70 arasi", AchievementCategory.Resource, 50, false);
            AddAchievement("extreme_gold", "Asiri Zenginlik", "Altin 95+'e ulastir", AchievementCategory.Resource, 40, false);
            AddAchievement("extreme_happiness", "Cennet", "Mutluluk 95+'e ulastir", AchievementCategory.Resource, 40, false);
            AddAchievement("extreme_military", "Ordular Ordusu", "Askeri 95+'e ulastir", AchievementCategory.Resource, 40, false);
            AddAchievement("extreme_faith", "Tanrinin Secilmisi", "Inanc 95+'e ulastir", AchievementCategory.Resource, 40, false);
            AddAchievement("roller_coaster", "Lunapark", "Bir turda 4 kaynak da degissin", AchievementCategory.Resource, 30, false);
            AddAchievement("stable_50", "Sabit Kaya", "50 tur hicbir kaynak 25 altina dusmesin", AchievementCategory.Resource, 80, false);

            // === KARAKTER BASARILARI (25) ===
            AddAchievement("meet_marcus", "Danismanla Tanisma", "Marcus ile tani", AchievementCategory.Character, 5, false);
            AddAchievement("marcus_friend", "Sadik Dost", "Marcus ile 50+ iliski", AchievementCategory.Character, 40, false);
            AddAchievement("marcus_enemy", "Ihanet", "Marcus ile -50 iliski", AchievementCategory.Character, 30, false);
            AddAchievement("meet_miriam", "Tuccar ile Tanisma", "Miriam ile tani", AchievementCategory.Character, 5, false);
            AddAchievement("miriam_partner", "Is Ortagi", "Miriam ile 50+ iliski", AchievementCategory.Character, 40, false);
            AddAchievement("miriam_rival", "Rakip", "Miriam ile -50 iliski", AchievementCategory.Character, 30, false);
            AddAchievement("meet_valerius", "General ile Tanisma", "Valerius ile tani", AchievementCategory.Character, 5, false);
            AddAchievement("valerius_loyal", "Sadik Komutan", "Valerius ile 50+ iliski", AchievementCategory.Character, 40, false);
            AddAchievement("valerius_coup", "Darbe!", "Valerius tarafindan devrildın", AchievementCategory.Character, 30, false);
            AddAchievement("meet_priest", "Rahiple Tanisma", "Rahip Basi ile tani", AchievementCategory.Character, 5, false);
            AddAchievement("priest_blessing", "Kutsandin", "Rahip ile 50+ iliski", AchievementCategory.Character, 40, false);
            AddAchievement("priest_enemy", "Aforoz", "Rahip ile -50 iliski", AchievementCategory.Character, 30, false);
            AddAchievement("meet_queen", "Kralice ile Tanisma", "Eleanor ile tani", AchievementCategory.Character, 10, false);
            AddAchievement("queen_love", "Mutlu Evlilik", "Eleanor ile 70+ iliski", AchievementCategory.Character, 50, false);
            AddAchievement("queen_hate", "Basarisiz Evlilik", "Eleanor ile -50 iliski", AchievementCategory.Character, 30, false);
            AddAchievement("meet_heir", "Veliaht ile Tanisma", "Edmund ile tani", AchievementCategory.Character, 10, false);
            AddAchievement("heir_proud", "Gurur Duyulan Ogul", "Edmund ile 70+ iliski", AchievementCategory.Character, 50, false);
            AddAchievement("heir_rebel", "Asi Veliaht", "Edmund ile -50 iliski", AchievementCategory.Character, 30, false);
            AddAchievement("social_king", "Sosyal Kelebek", "6 farkli karakterle tani", AchievementCategory.Character, 60, false);
            AddAchievement("everyone_friend", "Herkesin Dostu", "Tum karakterlerle 30+ iliski", AchievementCategory.Character, 100, false);
            AddAchievement("everyone_enemy", "Herkesin Dusmani", "Tum karakterlerle -30 iliski", AchievementCategory.Character, 100, false);
            AddAchievement("char_10_interactions", "Sık Gorusen", "Bir karakterle 10+ kez goruş", AchievementCategory.Character, 30, false);
            AddAchievement("char_25_interactions", "Yakin Arkadas", "Bir karakterle 25+ kez goruş", AchievementCategory.Character, 60, false);
            AddAchievement("char_perfect_relation", "Tam Guven", "Bir karakterle 100 iliski", AchievementCategory.Character, 75, false);
            AddAchievement("char_total_enemy", "Tam Dusmanlik", "Bir karakterle -100 iliski", AchievementCategory.Character, 75, false);

            // === HIKAYE BASARILARI (20) ===
            AddAchievement("golden_age", "Altin Cag", "Altin Cag sonuna ulas", AchievementCategory.Story, 100, false);
            AddAchievement("peaceful_reign", "Barisci", "Barisci Hukumdarlik sonuna ulas", AchievementCategory.Story, 80, false);
            AddAchievement("mighty_conqueror", "Fatih", "Guclu Fatih sonuna ulas", AchievementCategory.Story, 90, false);
            AddAchievement("holy_kingdom", "Kutsal Krallık", "Kutsal Krallik sonuna ulas", AchievementCategory.Story, 85, false);
            AddAchievement("dragon_slayer", "Ejderha Avcisi", "Ejderhayi yen", AchievementCategory.Story, 100, true);
            AddAchievement("grail_keeper", "Kase Koruyucusu", "Kutsal Kase'yi bul", AchievementCategory.Story, 100, true);
            AddAchievement("time_saver", "Zaman Kurtaricisi", "Geleceği değiştir", AchievementCategory.Story, 90, true);
            AddAchievement("legendary_king", "Efsanevi Kral", "100+ tur yasa", AchievementCategory.Story, 150, false);
            AddAchievement("balanced_ruler", "Dengeli Hukumdar", "Dengeli bitir", AchievementCategory.Story, 80, false);
            AddAchievement("crusade_join", "Hacli Sovalye", "Hacli seferine katil", AchievementCategory.Story, 50, false);
            AddAchievement("plague_survive", "Veba'dan Kurtulan", "Veba salginini atlatmayi basard", AchievementCategory.Story, 40, false);
            AddAchievement("rebellion_crush", "Isyan Bastiran", "Koylu isyanini bastir", AchievementCategory.Story, 35, false);
            AddAchievement("tournament_win", "Turnuva Sampiyonu", "Sovalye turnuvasini kazan", AchievementCategory.Story, 30, false);
            AddAchievement("witch_trial", "Cadi Avcisi", "Cadi davasini gerceklestir", AchievementCategory.Story, 25, false);
            AddAchievement("royal_wedding", "Kraliyet Dugunu", "Kralice ile evlen", AchievementCategory.Story, 40, false);
            AddAchievement("heir_born", "Veliaht Doğumu", "Bir veliaht sahibi ol", AchievementCategory.Story, 35, false);
            AddAchievement("trade_route", "Ticaret Yolu", "Yeni ticaret yolu aç", AchievementCategory.Story, 30, false);
            AddAchievement("church_built", "Kilise Insasi", "Buyuk kilise inşa et", AchievementCategory.Story, 35, false);
            AddAchievement("castle_upgrade", "Kale Guclendir", "Kaleyi guclendir", AchievementCategory.Story, 40, false);
            AddAchievement("all_story_events", "Hikaye Ustasi", "Tum ana hikaye olaylarini gor", AchievementCategory.Story, 200, true);

            // === OZEL BASARILAR (15) ===
            AddAchievement("first_game", "Hosgeldin", "Ilk oyununu oyna", AchievementCategory.Special, 5, false);
            AddAchievement("comeback", "Geri Donus", "20'nin altindan 80'e cik", AchievementCategory.Special, 50, false);
            AddAchievement("downfall", "Dusus", "80'den 20'nin altina dus", AchievementCategory.Special, 30, false);
            AddAchievement("all_max", "Mukemmel An", "Tum kaynaklar ayni anda 80+", AchievementCategory.Special, 100, true);
            AddAchievement("all_low", "Kritik An", "Tum kaynaklar ayni anda 30-", AchievementCategory.Special, 50, false);
            AddAchievement("lucky", "Sansli", "3 turda ust uste olumlu sonuc", AchievementCategory.Special, 40, false);
            AddAchievement("unlucky", "Sanssiz", "3 turda ust uste olumsuz sonuc", AchievementCategory.Special, 30, false);
            AddAchievement("pp_100", "Prestij Baslangici", "100 PP topla", AchievementCategory.Special, 20, false);
            AddAchievement("pp_500", "Prestij Ustasi", "500 PP topla", AchievementCategory.Special, 50, false);
            AddAchievement("pp_1000", "Prestij Efsanesi", "1000 PP topla", AchievementCategory.Special, 100, false);
            AddAchievement("unlock_era", "Yeni Ufuklar", "Yeni bir donem ac", AchievementCategory.Special, 30, false);
            AddAchievement("unlock_all_eras", "Zaman Gezgini", "Tum donemleri ac", AchievementCategory.Special, 200, false);
            AddAchievement("unlock_scenario", "Yeni Baslangic", "Yeni bir senaryo ac", AchievementCategory.Special, 25, false);
            AddAchievement("speedrun", "Hiz Kosusu", "30 turda zafer kazan", AchievementCategory.Special, 100, true);
            AddAchievement("completionist", "Tamamlayici", "Tum basarilari ac", AchievementCategory.Special, 500, true);

            // === GIZLI BASARILAR (15) ===
            AddAchievement("secret_poison", "Zehirli Son", "Danismanin tarafindan zehirlendin", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_betrayal", "Aile Ihaneti", "Veliaht tarafindan olduruldun", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_queen", "Kralice Komplosu", "Kralice tarafindan ihanete ugradin", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_merchant", "Dolandirdin", "Tuccar tarafindan dolandirdin", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_general", "Askeri Darbe", "General tarafindan devrildın", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_theocracy", "Teokrasi", "Din adamlari yonetimi ele gecirdi", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_laziness", "Tembellik", "Asiri mutluluktan coktun", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_inflation", "Hiperenflasyon", "Asiri zenginlikten coktun", AchievementCategory.Secret, 30, true);
            AddAchievement("secret_all_deaths", "Olum Ansiklopedisi", "Her sekilde ol", AchievementCategory.Secret, 200, true);
            AddAchievement("secret_no_swipe_left", "Sadece Sag", "30 tur sadece saga kaydir", AchievementCategory.Secret, 75, true);
            AddAchievement("secret_no_swipe_right", "Sadece Sol", "30 tur sadece sola kaydir", AchievementCategory.Secret, 75, true);
            AddAchievement("secret_alternate", "Kararsiz", "20 tur sag-sol-sag-sol sirayla", AchievementCategory.Secret, 60, true);
            AddAchievement("secret_turn_1_death", "Anlık Son", "Ilk turda ol", AchievementCategory.Secret, 50, true);
            AddAchievement("secret_all_extreme", "Asirilik", "Tum kaynaklari bir oyunda 90+ yap", AchievementCategory.Secret, 100, true);
            AddAchievement("secret_perfect_game", "Kusursuz Oyun", "100 tur hicbir kaynak 25-75 disina cikmadan", AchievementCategory.Secret, 300, true);

            Debug.Log($"[AchievementSystem] {_achievements.Count} basari yuklendi.");
        }

        private void AddAchievement(string id, string name, string description, AchievementCategory category, int ppReward, bool isSecret)
        {
            _achievements[id] = new Achievement
            {
                id = id,
                name = name,
                description = description,
                category = category,
                prestigeReward = ppReward,
                isSecret = isSecret
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Basariyi ac
        /// </summary>
        public bool UnlockAchievement(string achievementId)
        {
            if (_unlockedAchievements.Contains(achievementId))
                return false;

            if (!_achievements.TryGetValue(achievementId, out var achievement))
            {
                Debug.LogWarning($"[AchievementSystem] Basari bulunamadi: {achievementId}");
                return false;
            }

            _unlockedAchievements.Add(achievementId);

            // Prestige puani ver
            if (PrestigeManager.Instance != null)
            {
                PrestigeManager.Instance.AddPrestigePoints(achievement.prestigeReward);
            }

            OnAchievementUnlocked?.Invoke(achievement);
            OnProgressUpdated?.Invoke(UnlockedCount);

            Debug.Log($"[AchievementSystem] BASARI: {achievement.name} (+{achievement.prestigeReward} PP)");

            SaveAchievements();
            return true;
        }

        /// <summary>
        /// Basari acilmis mi kontrol et
        /// </summary>
        public bool IsUnlocked(string achievementId)
        {
            return _unlockedAchievements.Contains(achievementId);
        }

        /// <summary>
        /// Basari bilgisi al
        /// </summary>
        public Achievement GetAchievement(string achievementId)
        {
            return _achievements.TryGetValue(achievementId, out var achievement) ? achievement : null;
        }

        /// <summary>
        /// Kategoriye gore basarilari al
        /// </summary>
        public List<Achievement> GetAchievementsByCategory(AchievementCategory category)
        {
            return _achievements.Values
                .Where(a => a.category == category)
                .ToList();
        }

        /// <summary>
        /// Tum basarilari al
        /// </summary>
        public List<Achievement> GetAllAchievements()
        {
            return _achievements.Values.ToList();
        }

        /// <summary>
        /// Acilmis basarilari al
        /// </summary>
        public List<Achievement> GetUnlockedAchievements()
        {
            return _achievements.Values
                .Where(a => _unlockedAchievements.Contains(a.id))
                .ToList();
        }

        /// <summary>
        /// Oyun durumunu kontrol et ve basarilari ac
        /// </summary>
        public void CheckAchievements(GameStateData gameState)
        {
            // Hayatta kalma basarilari
            CheckSurvivalAchievements(gameState);

            // Kaynak basarilari
            CheckResourceAchievements(gameState);

            // Karakter basarilari
            CheckCharacterAchievements(gameState);

            // Ozel basarilar
            CheckSpecialAchievements(gameState);
        }

        /// <summary>
        /// Oyun sonu basari kontrolu
        /// </summary>
        public void CheckEndGameAchievements(GameStateData gameState, EndingType endingType)
        {
            // Ending bazli basarilar
            switch (endingType)
            {
                case EndingType.GoldenAge:
                    UnlockAchievement("golden_age");
                    break;
                case EndingType.PeacefulReign:
                    UnlockAchievement("peaceful_reign");
                    break;
                case EndingType.MightyConqueror:
                    UnlockAchievement("mighty_conqueror");
                    break;
                case EndingType.HolyKingdom:
                    UnlockAchievement("holy_kingdom");
                    break;
                case EndingType.DragonSlayer:
                    UnlockAchievement("dragon_slayer");
                    break;
                case EndingType.GrailKeeper:
                    UnlockAchievement("grail_keeper");
                    break;
                case EndingType.TimeSaver:
                    UnlockAchievement("time_saver");
                    break;
                case EndingType.LegendaryKing:
                    UnlockAchievement("legendary_king");
                    break;
                case EndingType.BalancedRuler:
                    UnlockAchievement("balanced_ruler");
                    break;
                case EndingType.PoisonedByAdvisor:
                    UnlockAchievement("secret_poison");
                    break;
                case EndingType.AssassinatedByHeir:
                    UnlockAchievement("secret_betrayal");
                    break;
                case EndingType.BetrayelByQueen:
                    UnlockAchievement("secret_queen");
                    break;
                case EndingType.MerchantScam:
                    UnlockAchievement("secret_merchant");
                    break;
                case EndingType.BetrayalByGeneral:
                    UnlockAchievement("secret_general");
                    break;
                case EndingType.TheocraticTakeover:
                    UnlockAchievement("secret_theocracy");
                    break;
                case EndingType.LazyDecline:
                    UnlockAchievement("secret_laziness");
                    break;
                case EndingType.EconomicCollapse:
                    UnlockAchievement("secret_inflation");
                    break;
            }

            // Zafer basarilari
            if (EndingSystem.IsVictory(endingType))
            {
                UnlockAchievement("first_victory");

                // Hiz kosusu
                if (gameState.turn <= 30)
                {
                    UnlockAchievement("speedrun");
                }
            }

            // Hizli olum
            if (gameState.turn <= 5)
            {
                UnlockAchievement("quick_death");
            }

            // Ilk tur olumu
            if (gameState.turn == 1)
            {
                UnlockAchievement("secret_turn_1_death");
            }
        }
        #endregion

        #region Private Achievement Checks
        private void CheckSurvivalAchievements(GameStateData gameState)
        {
            int turn = gameState.turn;

            if (turn >= 10) UnlockAchievement("survive_10");
            if (turn >= 25) UnlockAchievement("survive_25");
            if (turn >= 50) UnlockAchievement("survive_50");
            if (turn >= 75) UnlockAchievement("survive_75");
            if (turn >= 100) UnlockAchievement("survive_100");
            if (turn >= 150) UnlockAchievement("survive_150");
            if (turn >= 200) UnlockAchievement("survive_200");
        }

        private void CheckResourceAchievements(GameStateData gameState)
        {
            var res = gameState.resources;

            // Max kontrolu
            if (res.Gold >= 100) UnlockAchievement("gold_max");
            if (res.Happiness >= 100) UnlockAchievement("happiness_max");
            if (res.Military >= 100) UnlockAchievement("military_max");
            if (res.Faith >= 100) UnlockAchievement("faith_max");

            // Extreme kontrolu
            if (res.Gold >= 95) UnlockAchievement("extreme_gold");
            if (res.Happiness >= 95) UnlockAchievement("extreme_happiness");
            if (res.Military >= 95) UnlockAchievement("extreme_military");
            if (res.Faith >= 95) UnlockAchievement("extreme_faith");

            // Tum kaynaklar yuksek
            if (res.Gold >= 80 && res.Happiness >= 80 && res.Military >= 80 && res.Faith >= 80)
            {
                UnlockAchievement("all_max");
            }

            // Tum kaynaklar dusuk
            if (res.Gold <= 30 && res.Happiness <= 30 && res.Military <= 30 && res.Faith <= 30)
            {
                UnlockAchievement("all_low");
            }

            // Mukemmel denge
            if (res.Gold >= 45 && res.Gold <= 55 &&
                res.Happiness >= 45 && res.Happiness <= 55 &&
                res.Military >= 45 && res.Military <= 55 &&
                res.Faith >= 45 && res.Faith <= 55)
            {
                // Bu 30 tur kontrol gerektirir - state'de takip edilmeli
                // Simdilik basit kontrol
            }
        }

        private void CheckCharacterAchievements(GameStateData gameState)
        {
            foreach (var kvp in gameState.characterStates)
            {
                var charState = kvp.Value;
                string charId = kvp.Key;

                // Tanis basarilari
                if (charState.interactionCount > 0)
                {
                    if (charId == "marcus") UnlockAchievement("meet_marcus");
                    if (charId == "miriam") UnlockAchievement("meet_miriam");
                    if (charId == "valerius") UnlockAchievement("meet_valerius");
                    if (charId == "benedictus") UnlockAchievement("meet_priest");
                    if (charId == "eleanor") UnlockAchievement("meet_queen");
                    if (charId == "edmund") UnlockAchievement("meet_heir");
                }

                // Iliski basarilari
                if (charState.relationshipLevel >= 50)
                {
                    if (charId == "marcus") UnlockAchievement("marcus_friend");
                    if (charId == "miriam") UnlockAchievement("miriam_partner");
                    if (charId == "valerius") UnlockAchievement("valerius_loyal");
                    if (charId == "benedictus") UnlockAchievement("priest_blessing");
                }

                if (charState.relationshipLevel >= 70)
                {
                    if (charId == "eleanor") UnlockAchievement("queen_love");
                    if (charId == "edmund") UnlockAchievement("heir_proud");
                }

                if (charState.relationshipLevel <= -50)
                {
                    if (charId == "marcus") UnlockAchievement("marcus_enemy");
                    if (charId == "miriam") UnlockAchievement("miriam_rival");
                    if (charId == "benedictus") UnlockAchievement("priest_enemy");
                    if (charId == "eleanor") UnlockAchievement("queen_hate");
                    if (charId == "edmund") UnlockAchievement("heir_rebel");
                }

                // Etkilesim sayisi
                if (charState.interactionCount >= 10) UnlockAchievement("char_10_interactions");
                if (charState.interactionCount >= 25) UnlockAchievement("char_25_interactions");

                // Tam iliski
                if (charState.relationshipLevel >= 100) UnlockAchievement("char_perfect_relation");
                if (charState.relationshipLevel <= -100) UnlockAchievement("char_total_enemy");
            }

            // Sosyal kral
            if (gameState.characterStates.Count >= 6)
            {
                UnlockAchievement("social_king");
            }
        }

        private void CheckSpecialAchievements(GameStateData gameState)
        {
            // Flag bazli ozel basarilar
            if (gameState.HasFlag("crusade_joined")) UnlockAchievement("crusade_join");
            if (gameState.HasFlag("plague_survived")) UnlockAchievement("plague_survive");
            if (gameState.HasFlag("rebellion_crushed")) UnlockAchievement("rebellion_crush");
            if (gameState.HasFlag("tournament_won")) UnlockAchievement("tournament_win");
            if (gameState.HasFlag("witch_trial")) UnlockAchievement("witch_trial");
            if (gameState.HasFlag("married")) UnlockAchievement("royal_wedding");
            if (gameState.HasFlag("heir_born")) UnlockAchievement("heir_born");
            if (gameState.HasFlag("trade_route")) UnlockAchievement("trade_route");
            if (gameState.HasFlag("church_built")) UnlockAchievement("church_built");
            if (gameState.HasFlag("castle_upgraded")) UnlockAchievement("castle_upgrade");
        }
        #endregion

        #region Save/Load
        private void SaveAchievements()
        {
            string json = JsonUtility.ToJson(new AchievementSaveData { unlockedIds = _unlockedAchievements.ToList() });
            PlayerPrefs.SetString(Constants.SAVE_KEY_ACHIEVEMENTS, json);
            PlayerPrefs.Save();
        }

        private void LoadAchievements()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_ACHIEVEMENTS, "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var data = JsonUtility.FromJson<AchievementSaveData>(json);
                    _unlockedAchievements = new HashSet<string>(data.unlockedIds);
                }
                catch
                {
                    _unlockedAchievements = new HashSet<string>();
                }
            }

            Debug.Log($"[AchievementSystem] {_unlockedAchievements.Count}/{_achievements.Count} basari acilmis.");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Unlock Random Achievement")]
        private void DebugUnlockRandom()
        {
            var locked = _achievements.Keys.Except(_unlockedAchievements).ToList();
            if (locked.Count > 0)
            {
                UnlockAchievement(locked[UnityEngine.Random.Range(0, locked.Count)]);
            }
        }

        [ContextMenu("Reset All Achievements")]
        private void DebugResetAchievements()
        {
            _unlockedAchievements.Clear();
            SaveAchievements();
            Debug.Log("[AchievementSystem] Tum basarilar sifirlandi!");
        }

        [ContextMenu("Print Achievement Stats")]
        private void DebugPrintStats()
        {
            Debug.Log($"Unlocked: {UnlockedCount}/{TotalCount} ({CompletionPercentage:F1}%)");
            foreach (AchievementCategory cat in System.Enum.GetValues(typeof(AchievementCategory)))
            {
                var catAchievements = GetAchievementsByCategory(cat);
                var unlocked = catAchievements.Count(a => IsUnlocked(a.id));
                Debug.Log($"  {cat}: {unlocked}/{catAchievements.Count}");
            }
        }
#endif
        #endregion
    }

    #region Data Classes
    /// <summary>
    /// Basari veri yapisi
    /// </summary>
    [System.Serializable]
    public class Achievement
    {
        public string id;
        public string name;
        public string description;
        public AchievementCategory category;
        public int prestigeReward;
        public bool isSecret;
    }

    /// <summary>
    /// Basari kategorileri
    /// </summary>
    public enum AchievementCategory
    {
        Survival,   // Hayatta kalma
        Resource,   // Kaynak yonetimi
        Character,  // Karakter iliskileri
        Story,      // Hikaye tamamlama
        Special,    // Ozel basarilar
        Secret      // Gizli basarilar
    }

    /// <summary>
    /// Save data wrapper
    /// </summary>
    [System.Serializable]
    public class AchievementSaveData
    {
        public List<string> unlockedIds = new List<string>();
    }
    #endregion
}
