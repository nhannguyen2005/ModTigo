using System;
using System.Collections.Generic;
using System.Text;

namespace Mod
{
    /// <summary>
    /// AutoMap – Tự động di chuyển tới bản đồ đích theo đường đi ngắn nhất.
    /// Hỗ trợ: capsule dịch chuyển, nhặt đùi gà, thu đậu thần kỳ, lưu cài đặt.
    /// </summary>
    public class AutoMap : IActionListener
    {
        // ─────────────────────────────────────────────
        //  Singleton
        // ─────────────────────────────────────────────
        public static AutoMap _Instance;

        public static AutoMap getInstance()
        {
            if (_Instance == null)
                _Instance = new AutoMap();
            return _Instance;
        }

        // ─────────────────────────────────────────────
        //  Tên tất cả 187 bản đồ (0..186) (index = mapID)
        // ─────────────────────────────────────────────
        public static readonly string[] MapNames = new string[]
        {
            /* 000 */ "Làng Aru",
            /* 001 */ "Đồi hoa cúc",
            /* 002 */ "Thung lũng tre",
            /* 003 */ "Rừng nấm",
            /* 004 */ "Rừng xương",
            /* 005 */ "Đảo Kamê",
            /* 006 */ "Đông Karin",
            /* 007 */ "Làng Mori",
            /* 008 */ "Đồi nấm tím",
            /* 009 */ "Thị trấn Moori",
            /* 010 */ "Thung lũng Namếc",
            /* 011 */ "Thung lũng Maima",
            /* 012 */ "Vực Maima",
            /* 013 */ "Đảo Guru",
            /* 014 */ "Làng Kakarot",
            /* 015 */ "Đồi hoang",
            /* 016 */ "Làng Plant",
            /* 017 */ "Rừng nguyên sinh",
            /* 018 */ "Rừng thông Xayda",
            /* 019 */ "Thành phố Vegeta",
            /* 020 */ "Vách núi đen",
            /* 021 */ "Nhà Gôhan",
            /* 022 */ "Nhà Moori",
            /* 023 */ "Nhà Broly",
            /* 024 */ "Trạm tàu vũ trụ (Trái Đất)",
            /* 025 */ "Trạm tàu vũ trụ (Namếc)",
            /* 026 */ "Trạm tàu vũ trụ (Xayda)",
            /* 027 */ "Rừng Bamboo",
            /* 028 */ "Rừng dương xỉ",
            /* 029 */ "Nam Kamê",
            /* 030 */ "Đảo Bulông",
            /* 031 */ "Núi hoa vàng",
            /* 032 */ "Núi hoa tím",
            /* 033 */ "Nam Guru",
            /* 034 */ "Đông Nam Guru",
            /* 035 */ "Rừng cọ",
            /* 036 */ "Rừng đá",
            /* 037 */ "Thung lũng đen",
            /* 038 */ "Bờ vực đen",
            /* 039 */ "Vách núi Aru",
            /* 040 */ "Vách núi Moori",
            /* 041 */ "Vực Plant",
            /* 042 */ "Vách núi Aru (cổng)",
            /* 043 */ "Vách núi Moori (cổng)",
            /* 044 */ "Vách núi Kakarot (cổng)",
            /* 045 */ "Thần điện",
            /* 046 */ "Tháp Karin",
            /* 047 */ "Rừng Karin",
            /* 048 */ "Hành tinh Kaio",
            /* 049 */ "Phòng tập thời gian",
            /* 050 */ "Thánh địa Kaio",
            /* 051 */ "Đấu trường",
            /* 052 */ "Đại hội võ thuật",
            /* 053 */ "Tường thành 1",
            /* 054 */ "Tầng 3",
            /* 055 */ "Tầng 1",
            /* 056 */ "Tầng 2",
            /* 057 */ "Tầng 4",
            /* 058 */ "Tường thành 2",
            /* 059 */ "Tường thành 3",
            /* 060 */ "Trại độc nhãn 1",
            /* 061 */ "Trại độc nhãn 2",
            /* 062 */ "Trại độc nhãn 3",
            /* 063 */ "Trại lính Fide",
            /* 064 */ "Núi dây leo",
            /* 065 */ "Núi cây quỷ",
            /* 066 */ "Trại quỷ già",
            /* 067 */ "Vực chết",
            /* 068 */ "Thung lũng Nappa",
            /* 069 */ "Vực cấm",
            /* 070 */ "Núi Appule",
            /* 071 */ "Căn cứ Raspberry",
            /* 072 */ "Thung lũng Raspberry",
            /* 073 */ "Thung lũng chết",
            /* 074 */ "Đồi cây Fide",
            /* 075 */ "Khe núi tử thần",
            /* 076 */ "Núi đá",
            /* 077 */ "Rừng đá",
            /* 078 */ "Lãnh địa Fize",
            /* 079 */ "Núi khỉ đỏ",
            /* 080 */ "Núi khỉ vàng",
            /* 081 */ "Hang quỷ chim",
            /* 082 */ "Núi khỉ đen",
            /* 083 */ "Hang khỉ đen",
            /* 084 */ "Siêu Thị",
            /* 085 */ "Hành tinh M-2",
            /* 086 */ "Hành tinh Polaris",
            /* 087 */ "Hành tinh Cretaceous",
            /* 088 */ "Hành tinh Monmaasu",
            /* 089 */ "Hành tinh Rudeeze",
            /* 090 */ "Hành tinh Gelbo",
            /* 091 */ "Hành tinh Tigere",
            /* 092 */ "Thành phố phía đông",
            /* 093 */ "Thành phố phía nam",
            /* 094 */ "Đảo Balê",
            /* 095 */ "???",
            /* 096 */ "Cao nguyên",
            /* 097 */ "Thành phố phía bắc",
            /* 098 */ "Ngọn núi phía bắc",
            /* 099 */ "Thung lũng phía bắc",
            /* 100 */ "Thị trấn Ginder",
            /* 101 */ "???",
            /* 102 */ "Nhà Bunma",
            /* 103 */ "Võ đài Xên bọ hung",
            /* 104 */ "Sân sau siêu thị",
            /* 105 */ "Cánh đồng tuyết",
            /* 106 */ "Rừng tuyết",
            /* 107 */ "Núi tuyết",
            /* 108 */ "Dòng sông băng",
            /* 109 */ "Rừng băng",
            /* 110 */ "Hang băng",
            /* 111 */ "Đông Nam Karin",
            /* 112 */ "Võ đài Hạt Mít",
            /* 113 */ "Đại hội võ thuật (Siêu hạng)",
            /* 114 */ "Cổng phi thuyền",
            /* 115 */ "Phòng chờ",
            /* 116 */ "Thánh địa Kaio",
            /* 117 */ "Cửa Ải 1",
            /* 118 */ "Cửa Ải 2",
            /* 119 */ "Cửa Ải 3",
            /* 120 */ "Phòng chỉ huy",
            /* 121 */ "Đấu trường",
            /* 122 */ "Ngũ Hành Sơn",
            /* 123 */ "Ngũ Hành Sơn",
            /* 124 */ "Ngũ Hành Sơn",
            /* 125 */ "Võ đài Bang",
            /* 126 */ "Thành phố Santa",
            /* 127 */ "Cổng phi thuyền",
            /* 128 */ "Bụng Mabư",
            /* 129 */ "Đại hội võ thuật",
            /* 130 */ "Đại hội võ thuật Vũ Trụ",
            /* 131 */ "Hành tinh Yardart",
            /* 132 */ "Hành tinh Yardart 2",
            /* 133 */ "Hành tinh Yardart 3",
            /* 134 */ "Đại hội võ thuật Vũ Trụ 6-7",
            /* 135 */ "Động hải tặc",
            /* 136 */ "Hang Bạch Tuộc",
            /* 137 */ "Động kho báu",
            /* 138 */ "Cảng hải tặc",
            /* 139 */ "Hành tinh Potaufeu",
            /* 140 */ "Hang động Potaufeu",
            /* 141 */ "Con đường rắn độc",
            /* 142 */ "Con đường rắn độc",
            /* 143 */ "Con đường rắn độc",
            /* 144 */ "Hoang mạc",
            /* 145 */ "Võ đài Siêu Cấp",
            /* 146 */ "Tây Karin",
            /* 147 */ "Sa mạc",
            /* 148 */ "Lâu đài Lychee",
            /* 149 */ "Thành phố Santa",
            /* 150 */ "Lôi Đài",
            /* 151 */ "Hành tinh bóng tối",
            /* 152 */ "Vùng đất băng giá",
            /* 153 */ "Lãnh địa bang hội",
            /* 154 */ "Hành tinh Bill",
            /* 155 */ "Hành tinh ngục tù",
            /* 156 */ "Tây thánh địa",
            /* 157 */ "Đông thánh địa",
            /* 158 */ "Bắc thánh địa",
            /* 159 */ "Nam thánh địa",
            /* 160 */ "Khu hang động",
            /* 161 */ "Bìa rừng nguyên thủy",
            /* 162 */ "Rừng nguyên thủy",
            /* 163 */ "Làng Plant nguyên thủy",
            /* 164 */ "Tranh ngọc Namếc",
            /* 165 */ "Map Boss",
            /* 166 */ "Hành tinh ngục tù",
            /* 167 */ "Địa ngục tầng 1",
            /* 168 */ "Địa ngục tầng 2",
            /* 169 */ "Địa ngục tầng 3",
            /* 170 */ "Phòng tập thời gian",
            /* 171 */ "Địa ngục tầng 4",
            /* 172 */ "Phòng chờ thời gian",
            /* 173 */ "Cánh đồng tuyết 2",
            /* 174 */ "Rừng tuyết 2",
            /* 175 */ "Hang băng 2",
            /* 176 */ "NEW Nam Kamê",
            /* 177 */ "Rừng nguyên sinh",
            /* 178 */ "Ngọn núi phía bắc",
            /* 179 */ "Phòng thí nghiệm Myuu",
            /* 180 */ "Mù căng chải",
            /* 181 */ "Sa mạc hoang vu",
            /* 182 */ "Hành tinh Bill",
            /* 183 */ "Hành tinh Bill",
            /* 184 */ "Tường thành cổ 1",
            /* 185 */ "Tường thành cổ 2",
            /* 186 */ "Đấu trường thành cổ",
        };

        // ─────────────────────────────────────────────
        //  Nhóm map theo hành tinh (dùng để hiển thị menu)
        //  Thứ tự: cổng vào → làng → vùng phụ → vùng xa
        // ─────────────────────────────────────────────

        // === TRÁI ĐẤT ===
        // Cổng: 42 → Làng: 0,1,2,3,4,5,6 → Dungeon: 27..30
        // → Tháp: 47,46,45 → Kaio: 48 → Tường thành: 53..62
        private static readonly int[] idMapsTraiDat = new int[]
        {
            42,         // Vách núi Aru (cổng hành tinh)
            21,         // Nhà Gôhan
            0,          // Làng Aru
            1,          // Đồi hoa cúc
            2,          // Thung lũng tre
            3,          // Rừng nấm
            4,          // Rừng xương
            5,          // Đảo Kamê
            6,          // Đông Karin
            27,         // Rừng Bamboo
            28,         // Rừng dương xỉ
            29,         // Nam Kamê
            30,         // Đảo Bulông
            47,         // Rừng Karin
            46,         // Tháp Karin
            45,         // Thần điện
            48,         // Hành tinh Kaio
            24,         // Trạm tàu vũ trụ (Trái Đất)
            53,         // Tường thành 1
            58,         // Tường thành 2
            59,         // Tường thành 3
            60,         // Trại độc nhãn 1
            61,         // Trại độc nhãn 2
            62,         // Trại độc nhãn 3
            55,         // Tầng 1
            56,         // Tầng 2
            54,         // Tầng 3
            57,         // Tầng 4
        };

        // === NAMẾC ===
        // Cổng: 43 → Làng: 7..13 → Dungeon: 31..34
        private static readonly int[] idMapsNamek = new int[]
        {
            43,         // Vách núi Moori (cổng hành tinh)
            22,         // Nhà Moori
            7,          // Làng Mori
            8,          // Đồi nấm tím
            9,          // Thị trấn Moori
            11,         // Thung lũng Maima
            12,         // Vực Maima
            13,         // Đảo Guru
            10,         // Thung lũng Namếc
            31,         // Núi hoa vàng
            32,         // Núi hoa tím
            33,         // Nam Guru
            34,         // Đông Nam Guru
            25,         // Trạm tàu vũ trụ (Namếc)
        };

        // === XAYDA ===
        // Cổng: 44 → Làng: 14..20 → Dungeon: 35..38
        // → Đại hội: 52 → Siêu hạng: 113 → Freeza: 127,129
        private static readonly int[] idMapsXayda = new int[]
        {
            44,         // Vách núi Kakarot (cổng hành tinh)
            23,         // Nhà Broly
            14,         // Làng Kakarot
            15,         // Đồi hoang
            16,         // Làng Plant
            17,         // Rừng nguyên sinh
            18,         // Rừng thông Xayda
            20,         // Vách núi đen
            19,         // Thành phố Vegeta
            35,         // Rừng cọ
            36,         // Rừng đá
            37,         // Thung lũng đen
            38,         // Bờ vực đen
            52,         // Đại hội võ thuật
            26,         // Trạm tàu vũ trụ (Xayda)
            84,         // Siêu Thị
            113,        // Đại hội võ thuật (Siêu hạng)
            127,        // Thành phố Freeza
            129,        // Đảo băng giá
        };

        // === FIDE (HÀNH TINH NAPPA) ===
        // Cổng: 68 → Vùng: 63..83 → Núi khỉ: 79..83,131..133
        private static readonly int[] idMapsNappa = new int[]
        {
            68,         // Thung lũng Nappa
            69,         // Vực cấm
            70,         // Núi Appule
            71,         // Căn cứ Raspberry
            72,         // Thung lũng Raspberry
            64,         // Núi dây leo
            65,         // Núi cây quỷ
            63,         // Trại lính Fide
            66,         // Trại quỷ già
            67,         // Vực chết
            73,         // Thung lũng chết
            74,         // Đồi cây Fide
            75,         // Khe núi tử thần
            76,         // Núi đá
            77,         // Rừng đá
            79,         // Núi khỉ đỏ
            80,         // Núi khỉ vàng
            81,         // Hang quỷ chim
            82,         // Núi khỉ đen
            83,         // Hang khỉ đen
            131,        // Rừng khỉ
            132,        // Hang khỉ vàng
            133,        // Thung lũng khỉ
        };

        // === TƯƠNG LAI ===
        // Cổng: 102 → Vùng: 92..100,103
        private static readonly int[] idMapsTuongLai = new int[]
        {
            102,        // Nhà Bunma
            92,         // Thành phố phía đông
            93,         // Thành phố phía nam
            94,         // Đảo Balê
            96,         // Cao nguyên
            97,         // Thành phố phía bắc
            98,         // Ngọn núi phía bắc
            99,         // Thung lũng phía bắc
            100,        // Thị trấn Ginder
            103,        // Võ đài Xên bọ hung
        };

        // === COLD (BĂNG) ===
        // Cổng: 109 → Vùng: 105..110
        private static readonly int[] idMapsCold = new int[]
        {
            109,        // Rừng băng (cổng)
            108,        // Dòng sông băng
            107,        // Núi tuyết
            110,        // Hang băng
            106,        // Rừng tuyết
            105,        // Cánh đồng tuyết
        };

        // === HẢI TẶC ===
        private static readonly int[] idMapsHaiTac = new int[]
        {
            138, // Cảng hải tặc
            135, // Động hải tặc
            136, // Hang Bạch Tuộc
            137, // Động kho báu
        };

        // === POTAUFEU ===
        private static readonly int[] idMapsPotaufeu = new int[]
        {
            139, // Hành tinh Potaufeu
            140, // Hang động Potaufeu
            141, // Con đường rắn độc
            142, // Con đường rắn độc
            143, // Con đường rắn độc
        };

        // === NGUYÊN THỦY ===
        private static readonly int[] idMapsNguyenThuy = new int[]
        {
            161, // Bìa rừng nguyên thủy
            162, // Rừng nguyên thủy
            163, // Làng Plant nguyên thủy
            160, // Khu hang động
        };

        // === NGỤC TÙ ===
        private static readonly int[] idMapsNguTu = new int[]
        {
            155, // Hành tinh ngục tù
            166, // Hành tinh ngục tù
            167, // Địa ngục tầng 1
            168, // Địa ngục tầng 2
            169, // Địa ngục tầng 3
            171, // Địa ngục tầng 4
        };

        // === BILL / BÓNG TỐI ===
        private static readonly int[] idMapsBill = new int[]
        {
            154, // Hành tinh Bill
            182, // Hành tinh Bill
            183, // Hành tinh Bill
            151, // Hành tinh bóng tối
            152, // Vùng đất băng giá
        };

        // === THÁNH ĐỊA ===
        private static readonly int[] idMapsThanhDia = new int[]
        {
            156, // Tây thánh địa
            157, // Đông thánh địa
            158, // Bắc thánh địa
            159, // Nam thánh địa
        };

        // === COLD 2 ===
        private static readonly int[] idMapsCold2 = new int[]
        {
            173, // Cánh đồng tuyết 2
            174, // Rừng tuyết 2
            175, // Hang băng 2
        };

        // === KHÁC (đặc biệt / sự kiện / võ đài) ===
        private static readonly int[] idMapsKhac = new int[]
        {
            39,  // Vách núi Aru
            40,  // Vách núi Moori
            41,  // Vực Plant
            49,  // Phòng tập thời gian
            50,  // Thánh địa Kaio
            51,  // Đấu trường
            78,  // Lãnh địa Fize
            85,  // Hành tinh M-2
            86,  // Hành tinh Polaris
            87,  // Hành tinh Cretaceous
            88,  // Hành tinh Monmaasu
            89,  // Hành tinh Rudeeze
            90,  // Hành tinh Gelbo
            91,  // Hành tinh Tigere
            95,  // ???
            101, // ???
            104, // Sân sau siêu thị
            111, // Đông Nam Karin
            112, // Võ đài Hạt Mít
            114, // Cổng phi thuyền
            115, // Phòng chờ
            116, // Thánh địa Kaio
            117, // Cửa Ải 1
            118, // Cửa Ải 2
            119, // Cửa Ải 3
            120, // Phòng chỉ huy
            121, // Đấu trường
            122, // Ngũ Hành Sơn
            123, // Ngũ Hành Sơn
            124, // Ngũ Hành Sơn
            125, // Võ đài Bang
            126, // Thành phố Santa
            128, // Bụng Mabư
            130, // Đại hội võ thuật Vũ Trụ
            134, // Đại hội võ thuật Vũ Trụ 6-7
            144, // Hoang mạc
            145, // Võ đài Siêu Cấp
            146, // Tây Karin
            147, // Sa mạc
            148, // Lâu đài Lychee
            149, // Thành phố Santa
            150, // Lôi Đài
            153, // Lãnh địa bang hội
            164, // Tranh ngọc Namếc
            165, // Map Boss
            170, // Phòng tập thời gian
            172, // Phòng chờ thời gian
            176, // NEW Nam Kamê
            177, // Rừng nguyên sinh
            178, // Ngọn núi phía bắc
            179, // Phòng thí nghiệm Myuu
            180, // Mù căng chải
            181, // Sa mạc hoang vu
            184, // Tường thành cổ 1
            185, // Tường thành cổ 2
            186, // Đấu trường thành cổ
        };

        // ─────────────────────────────────────────────
        //  Trạng thái runtime
        // ─────────────────────────────────────────────
        public static bool isXmaping;           // Đang chạy auto map
        public static int IdMapEnd = -1;       // MapID đích cần đến (-1 khi không chạy)

        public static bool isEatChicken = true;   // Tự động nhặt đùi gà
        private static bool isHarvestPean = false;  // Tự động thu đậu thần kỳ
        private static bool isUseCapsule = true;   // Dùng capsule dịch chuyển
        private static bool isUsingCapsule;         // Đang trong quá trình dùng capsule
        private static bool isOpeningPanel;         // Đang mở panel chọn map capsule
        private static long lastTimeOpenedPanel;
        private static bool isSaveData;             // Lưu cài đặt vào RMS
        private static long lastWaitTime;

        // Waypoints cổng map (tọa độ x, y)
        private static int[] wayPointMapLeft;
        private static int[] wayPointMapCenter;
        private static int[] wayPointMapRight;
        private static int lastProcessedMap = -1;
        private static bool isProcessingMapChange;
        private static float lastMapChangeTime;
        private static float lastErrorTime;
        private static float lastNpcIndexActionTime;

        // ─────────────────────────────────────────────
        //  Đồ thị liên kết bản đồ
        //  key = mapID hiện tại → List<NextMap> các ô kề
        // ─────────────────────────────────────────────
        private static readonly Dictionary<int, List<NextMap>> linkMaps =
            new Dictionary<int, List<NextMap>>();

        // Menu hành tinh: tên → mảng mapID
        private static readonly Dictionary<string, int[]> planetDictionary =
            new Dictionary<string, int[]>();

        // ─────────────────────────────────────────────
        //  Constructor tĩnh – khởi tạo toàn bộ đồ thị
        // ─────────────────────────────────────────────
        static AutoMap()
        {
            LoadLinkMapsXmap();
            LoadNPCLinkMapsXmap();
            AddPlanetXmap();
            LoadData();
        }

        // ─────────────────────────────────────────────
        //  VÒNG LẶP UPDATE (gọi mỗi frame từ game loop)
        // ─────────────────────────────────────────────
        public static void Update()
        {
            if (global::Char.myCharz() == null)
                return;

            if (!isXmaping && !isEatChicken && !isHarvestPean)
                return;

            // Nếu nhân vật chết → chờ hồi sinh rồi mới tiếp tục
            if (Char.myCharz().meDead)
                lastWaitTime = mSystem.currentTimeMillis() + 1000L;

            // Đã tới map đích → dừng
            if (isXmaping && IdMapEnd >= 0 && TileMap.mapID == IdMapEnd)
            {
                FinishXmap();
                return;
            }

            bool pickedItem = false;

            // ── Nhặt đùi gà + thu đậu (chỉ trên map nhà, mapID 21/22/23) ──
            bool isHomePlanetMap = (TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 23);
            if (isHomePlanetMap)
            {
                // Nhặt đùi gà (template id = 74)
                if (isEatChicken && GameScr.vItemMap != null)
                {
                    for (int i = 0; i < GameScr.vItemMap.size(); i++)
                    {
                        if (i >= GameScr.vItemMap.size()) break;
                        ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                        if (itemMap == null || itemMap.template == null) continue;
                        bool canPick = (itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1)
                                       && itemMap.template.id == 74;
                        if (canPick)
                        {
                            pickedItem = true;
                            Char.myCharz().itemFocus = itemMap;
                            if (mSystem.currentTimeMillis() - lastWaitTime > 600L)
                            {
                                lastWaitTime = mSystem.currentTimeMillis();
                                Service.gI().pickItem(Char.myCharz().itemFocus.itemMapID);
                                return;
                            }
                        }
                    }
                }

                // Thu đậu thần kỳ (mở menu cây thần kỳ NPC id 4)
                if (isXmaping && isHarvestPean
                    && GameScr.hpPotion < 10
                    && GameScr.gI() != null
                    && GameScr.gI().magicTree != null
                    && GameScr.gI().magicTree.currPeas > 0
                    && mSystem.currentTimeMillis() - lastWaitTime > 500L)
                {
                    lastWaitTime = mSystem.currentTimeMillis();
                    Service.gI().openMenu(4);
                    Service.gI().menu(4, 0, 0);
                }
            }

            // ── Kiểm tra có nên di chuyển không ──
            bool shouldMove = isXmaping
                && !pickedItem
                && mSystem.currentTimeMillis() - lastWaitTime > 1000L
                && GameCanvas.gameTick % 20 == 0;

            if (!shouldMove) return;

            // ── Xử lý đặc biệt: khu vực Tương lai (27-29) ──
            // Map 27/28/29 có NPC 38 làm cổng sang Tương lai;
            // nếu NPC chưa load thì đi lại map 28 rồi mới tìm.
            bool goNext = true;
            if (isFutureMap(IdMapEnd))
            {
                if (goNext && TileMap.mapID == 27 && GameScr.findNPCInMap(38) == null)
                { goNext = false; UpdateXmap(28); }

                if (goNext && TileMap.mapID == 29 && GameScr.findNPCInMap(38) == null)
                { goNext = false; UpdateXmap(28); }

                if (goNext && TileMap.mapID == 28 && GameScr.findNPCInMap(38) == null)
                {
                    goNext = false;
                    // Nếu nhân vật đứng bên trái → qua 29, ngược lại → qua 27
                    UpdateXmap(Char.myCharz().cx < TileMap.pxw / 2 ? 29 : 27);
                }
            }

            if (goNext)
                UpdateXmap(IdMapEnd);
        }

        // ─────────────────────────────────────────────
        //  IActionListener – xử lý sự kiện từ menu
        // ─────────────────────────────────────────────
        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1: // Mở menu chọn hành tinh
                    ShowPlanetMenu();
                    break;

                case 2: // Bật/tắt nhặt đùi gà
                    isEatChicken = !isEatChicken;
                    GameScr.info1.addInfo("Ăn Đùi Gà\n" + (isEatChicken ? "ON" : "OFF"), 0);
                    if (isSaveData) Rms.saveRMSInt("AutoMapIsEatChicken", isEatChicken ? 1 : 0);
                    break;

                case 3: // Bật/tắt thu đậu thần kỳ
                    isHarvestPean = !isHarvestPean;
                    GameScr.info1.addInfo("Thu Đậu\n" + (isHarvestPean ? "ON" : "OFF"), 0);
                    if (isSaveData) Rms.saveRMSInt("AutoMapIsHarvestPean", isHarvestPean ? 1 : 0);
                    break;

                case 4: // Bật/tắt dùng capsule
                    isUseCapsule = !isUseCapsule;
                    GameScr.info1.addInfo("Sử Dụng Capsule\n" + (isUseCapsule ? "ON" : "OFF"), 0);
                    if (isSaveData) Rms.saveRMSInt("AutoMapIsUseCsb", isUseCapsule ? 1 : 0);
                    break;

                case 5: // Bật/tắt lưu cài đặt
                    isSaveData = !isSaveData;
                    GameScr.info1.addInfo("Lưu Cài Đặt Auto Map\n" + (isSaveData ? "ON" : "OFF"), 0);
                    Rms.saveRMSInt("AutoMapIsSaveRms", isSaveData ? 1 : 0);
                    if (isSaveData) SaveData();
                    break;

                case 6: // Mở menu danh sách map trong hành tinh
                    ShowMapsMenu((int[])p);
                    break;

                case 7: // Bắt đầu đi đến mapID đã chọn
                    isXmaping = true;
                    IdMapEnd = (int)p;
                    GameScr.info1.addInfo("Đi tới: " + GetMapDisplayName(IdMapEnd), 0);
                    break;
            }
        }

        // ─────────────────────────────────────────────
        //  MENU HIỂN THỊ
        // ─────────────────────────────────────────────

        /// <summary>Mở menu chính của AutoMap.</summary>
        public static void ShowMenu()
        {
            LoadData();
            MyVector items = new MyVector();
            items.addElement(new Command("Chọn Map", getInstance(), 1, null));
            items.addElement(new Command("Ăn Đùi Gà\n" + (isEatChicken ? "ON" : "OFF"), getInstance(), 2, null));
            items.addElement(new Command("Thu Đậu\n" + (isHarvestPean ? "ON" : "OFF"), getInstance(), 3, null));
            items.addElement(new Command("Sử Dụng Capsule\n" + (isUseCapsule ? "ON" : "OFF"), getInstance(), 4, null));
            items.addElement(new Command("Lưu Cài Đặt\n" + (isSaveData ? "ON" : "OFF"), getInstance(), 5, null));
            GameCanvas.menu.startAt(items, 3);
        }

        private static void ShowPlanetMenu()
        {
            MyVector items = new MyVector();
            foreach (KeyValuePair<string, int[]> kv in planetDictionary)
                items.addElement(new Command(kv.Key, getInstance(), 6, kv.Value));
            GameCanvas.menu.startAt(items, 3);
        }

        private static void ShowMapsMenu(int[] mapIDs)
        {
            int gender = Char.myCharz().cgender;
            MyVector items = new MyVector();
            for (int i = 0; i < mapIDs.Length; i++)
            {
                int id = mapIDs[i];
                // Lọc nhà theo giới tính (21=Gôhan/TĐ, 22=Moori/Namếc, 23=Broly/Xayda)
                bool skip = (gender == 0 && (id == 22 || id == 23))
                          || (gender == 1 && (id == 21 || id == 23))
                          || (gender == 2 && (id == 21 || id == 22));
                if (!skip)
                    items.addElement(new Command(GetMapDisplayName(id), getInstance(), 7, id));
            }
            GameCanvas.menu.startAt(items, 3);
        }

        // ─────────────────────────────────────────────
        //  API CÔNG KHAI
        // ─────────────────────────────────────────────

        /// <summary>Bắt đầu auto-map đến mapID chỉ định.</summary>
       public static bool xmapErrr;
        public static void StartRunToMapId(int mapID)
        {
            isXmaping = true;
            IdMapEnd = mapID;
            lastProcessedMap = -1;
            isProcessingMapChange = false;
            xmapErrr = false;
        }

        /// <summary>Dừng auto-map, reset trạng thái.</summary>
        public static void FinishXmap()
        {
            isXmaping = false;
            isUsingCapsule = false;
            isOpeningPanel = false;
        }

        // Dịch chuyển về các cổng map (dùng khi gọi ngoài class)
        public static void LoadMapLeft() => LoadMap(0);
        public static void LoadMapCenter() => LoadMap(2);
        public static void LoadMapRight() => LoadMap(1);

        // ─────────────────────────────────────────────
        //  LOGIC TÌM ĐƯỜNG & DI CHUYỂN
        // ─────────────────────────────────────────────

        /// <summary>
        /// Tính đường đi đến <paramref name="mapID"/> rồi thực thi bước tiếp theo.
        /// Nếu có capsule và đường dài → dùng capsule.
        /// </summary>
        private static void UpdateXmap(int mapID)
        {
            // Đảm bảo Siêu Thị (84) luôn có cầu nối về nhà theo giới tính mà không tạo mới list mỗi tick
            int targetHomeMap = 24 + Char.myCharz().cgender;
            if (!linkMaps.ContainsKey(84))
            {
                linkMaps[84] = new List<NextMap> { new NextMap(targetHomeMap, 10, 0) };
            }
            else if (linkMaps[84].Count > 0 && linkMaps[84][0].MapID != targetHomeMap)
            {
                linkMaps[84][0].MapID = targetHomeMap;
            }

            int[] path = FindWay(mapID);
            if (path == null)
            {
                GameScr.info1.addInfo("Không thể tìm thấy đường đi", 0);
                return;
            }

            // ── Capsule dịch chuyển (nếu đường > 3 bước) ──
            if (isUseCapsule)
            {
                // Chưa dùng capsule → thử tìm capsule trong túi
                if (!isUsingCapsule && path.Length > 3)
                {
                    foreach (Item item in Char.myCharz().arrItemBag)
                    {
                        if (item != null && (item.template.id == 194
                            || (item.template.id == 193 && item.quantity > 10)))
                        {
                            isUsingCapsule = true;
                            isOpeningPanel = false;
                            lastTimeOpenedPanel = mSystem.currentTimeMillis();
                            if (GameCanvas.panel != null)
                            {
                                GameCanvas.panel.mapNames = null;
                            }
                            Service.gI().useItem(0, 1, -1, item.template.id);
                            return;
                        }
                    }
                }

                // Capsule đang mở, chờ panel xuất hiện
                if (isUsingCapsule && !isOpeningPanel
                    && (GameCanvas.panel == null || GameCanvas.panel.mapNames == null
                        || mSystem.currentTimeMillis() - lastTimeOpenedPanel < 500L))
                    return;

                // Panel đã mở → tìm map xa nhất trong panel rồi chọn
                if (isUsingCapsule && !isOpeningPanel && GameCanvas.panel != null && GameCanvas.panel.mapNames != null)
                {
                    for (int j = path.Length - 1; j >= 2; j--)
                    {
                        if (path[j] < 0 || TileMap.mapNames == null || path[j] >= TileMap.mapNames.Length) continue;
                        string pMapName = TileMap.mapNames[path[j]];
                        if (string.IsNullOrEmpty(pMapName)) continue;

                        for (int k = 0; k < GameCanvas.panel.mapNames.Length; k++)
                        {
                            if (GameCanvas.panel.mapNames[k] != null && GameCanvas.panel.mapNames[k].Contains(pMapName))
                            {
                                isOpeningPanel = true;
                                Service.gI().requestMapSelect(k);
                                return;
                            }
                        }
                    }
                    isOpeningPanel = true;
                }
            }

            // ── Di chuyển thông thường (đến ô kề tiếp theo) ──
            if (path.Length > 1 && TileMap.mapID == path[0] && !Char.ischangingMap && !Controller.isStopReadMessage)
                Goto(path[1]);
        }

        /// <summary>Gọi NextMap.GotoMap() cho ô kề có MapID = mapID.</summary>
        private static void Goto(int mapID)
        {
            if (!linkMaps.ContainsKey(TileMap.mapID))
            {
                GameScr.info1.addInfo("Bản đồ hiện tại không có dữ liệu di chuyển", 0);
                FinishXmap();
                return;
            }

            foreach (NextMap nm in linkMaps[TileMap.mapID])
            {
                if (nm != null && nm.MapID == mapID)
                {
                    lastWaitTime = mSystem.currentTimeMillis();
                    nm.GotoMap();
                    return;
                }
            }
            GameScr.info1.addInfo("Không thể thực hiện", 0);
        }

        // ── BFS tìm đường đi ngắn nhất (cực nhanh, không lag, không đệ quy) ──

        public static string GetMapName(int mapId) => (mapId >= 0 && mapId < MapNames.Length) ? MapNames[mapId] : $"Map {mapId}";

        private static int[] FindWay(int mapIDEnd)
        {
            if (TileMap.mapID == mapIDEnd)
                return new int[] { mapIDEnd };

            if (!linkMaps.ContainsKey(TileMap.mapID) || !linkMaps.ContainsKey(mapIDEnd))
                return null;

            Queue<int> queue = new Queue<int>();
            Dictionary<int, int> parent = new Dictionary<int, int>();
            HashSet<int> visited = new HashSet<int>();

            int start = TileMap.mapID;
            queue.Enqueue(start);
            visited.Add(start);

            bool found = false;
            while (queue.Count > 0)
            {
                int curr = queue.Dequeue();
                if (curr == mapIDEnd)
                {
                    found = true;
                    break;
                }

                if (!linkMaps.ContainsKey(curr)) continue;

                foreach (NextMap nm in linkMaps[curr])
                {
                    if (nm == null) continue;
                    int next = nm.MapID;

                    // Bỏ qua map Cold nếu chưa đủ cấp nhiệm vụ
                    if (Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId <= 30 && (next >= 105 && next <= 110))
                        continue;

                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        parent[next] = curr;
                        queue.Enqueue(next);
                    }
                }
            }

            if (!found) return null;

            // Truy vết đường đi ngắn nhất từ map đích về map xuất phát
            List<int> path = new List<int>();
            int step = mapIDEnd;
            while (step != start)
            {
                path.Add(step);
                if (!parent.ContainsKey(step)) return null;
                step = parent[step];
            }
            path.Add(start);
            path.Reverse();

            return path.ToArray();
        }

        // ─────────────────────────────────────────────
        //  TELEPORT & WAYPOINT
        // ─────────────────────────────────────────────

        private static void LoadMap(int position)
        {
            if (isNRDMap(TileMap.mapID))
            {
                TeleportInNRDMap(position);
                return;
            }

            LoadWaypointsInMap();
            bool hasWaypoint = false;
            switch (position)
            {
                case 0: // Trái (Left)
                    if (wayPointMapLeft[0] != 0)
                    {
                        TeleportTo(wayPointMapLeft[0], wayPointMapLeft[1]);
                        hasWaypoint = true;
                    }
                    else
                    {
                        TeleportTo(60, GetYGround(60));
                    }
                    break;
                case 1: // Phải (Right)
                    int rx = TileMap.pxw - 60;
                    if (wayPointMapRight[0] != 0)
                    {
                        TeleportTo(wayPointMapRight[0], wayPointMapRight[1]);
                        hasWaypoint = true;
                    }
                    else
                    {
                        TeleportTo(rx, GetYGround(rx));
                    }
                    break;
                case 2: // Giữa (Center)
                    int cx = TileMap.pxw / 2;
                    if (wayPointMapCenter[0] != 0)
                    {
                        TeleportTo(wayPointMapCenter[0], wayPointMapCenter[1]);
                        hasWaypoint = true;
                    }
                    else
                    {
                        TeleportTo(cx, GetYGround(cx));
                    }
                    break;
            }

            if (hasWaypoint)
            {
                // Map 0,7,14 dùng offline; còn lại dùng online
                if (TileMap.mapID != 7 && TileMap.mapID != 14 && TileMap.mapID != 0)
                    Service.gI().requestChangeMap();
                else
                    Service.gI().getMapOffline();
            }
        }

        private static void TeleportInNRDMap(int position)
        {
            if (position == 0)
            {
                TeleportTo(60, GetYGround(60));
            }
            else if (position != 2)
            {
                TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
            }
            else
            {
                // Center → dịch đến NPC id 30-36
                for (int i = 0; i < GameScr.vNpc.size(); i++)
                {
                    Npc npc = (Npc)GameScr.vNpc.elementAt(i);
                    if (npc.template.npcTemplateId >= 30 && npc.template.npcTemplateId <= 36)
                    {
                        Char.myCharz().npcFocus = npc;
                        TeleportTo(npc.cx, npc.cy - 3);
                        break;
                    }
                }
            }
        }

        private static void LoadWaypointsInMap()
        {
            ResetSavedWaypoints();
            if (TileMap.vGo == null) return;
            int count = TileMap.vGo.size();
            if (count != 2)
            {
                for (int i = 0; i < count; i++)
                {
                    if (i >= TileMap.vGo.size()) break;
                    Waypoint wp = (Waypoint)TileMap.vGo.elementAt(i);
                    if (wp == null) continue;
                    if (wp.maxX < 60)
                    { wayPointMapLeft[0] = (int)(wp.minX + 15); wayPointMapLeft[1] = (int)wp.maxY; }
                    else if ((int)wp.maxX > TileMap.pxw - 60)
                    { wayPointMapRight[0] = (int)(wp.maxX - 15); wayPointMapRight[1] = (int)wp.maxY; }
                    else
                    { wayPointMapCenter[0] = (int)(wp.minX + 15); wayPointMapCenter[1] = (int)wp.maxY; }
                }
            }
            else
            {
                Waypoint a = (Waypoint)TileMap.vGo.elementAt(0);
                Waypoint b = (Waypoint)TileMap.vGo.elementAt(1);
                if (a == null || b == null) return;
                // Cả hai cùng bên → trái + phải
                if ((a.maxX < 60 && b.maxX < 60)
                    || ((int)a.minX > TileMap.pxw - 60 && (int)b.minX > TileMap.pxw - 60))
                {
                    wayPointMapLeft[0] = (int)(a.minX + 15); wayPointMapLeft[1] = (int)a.maxY;
                    wayPointMapRight[0] = (int)(b.maxX - 15); wayPointMapRight[1] = (int)b.maxY;
                }
                else if (a.maxX < b.maxX)
                {
                    wayPointMapLeft[0] = (int)(a.minX + 15); wayPointMapLeft[1] = (int)a.maxY;
                    wayPointMapRight[0] = (int)(b.maxX - 15); wayPointMapRight[1] = (int)b.maxY;
                }
                else
                {
                    wayPointMapLeft[0] = (int)(b.minX + 15); wayPointMapLeft[1] = (int)b.maxY;
                    wayPointMapRight[0] = (int)(a.maxX - 15); wayPointMapRight[1] = (int)a.maxY;
                }
            }
        }

        public static int GetYGround(int x)
        {
            int y = 50;
            for (int i = 0; i < 30; i++)
            {
                y += 24;
                if (TileMap.tileTypeAt(x, y, 2))
                {
                    if (y % 24 != 0) y -= y % 24;
                    break;
                }
            }
            return y;
        }

        public static void TeleportTo(int x, int y)
        {
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
            if (!GameScr.canAutoPlay)
            {
                Char.myCharz().cx = x;
                Char.myCharz().cy = y + 1;
                Service.gI().charMove();
                Char.myCharz().cx = x;
                Char.myCharz().cy = y;
                Service.gI().charMove();
            }
        }

        private static void ResetSavedWaypoints()
        {
            wayPointMapLeft = new int[2];
            wayPointMapCenter = new int[2];
            wayPointMapRight = new int[2];
        }

        // ─────────────────────────────────────────────
        //  HELPER
        // ─────────────────────────────────────────────

        private static bool isNRDMap(int mapID) => mapID >= 85 && mapID <= 91;

        private static bool isFutureMap(int mapID)
        {
            foreach (int id in idMapsTuongLai)
                if (id == mapID) return true;
            return false;
        }

        /// <summary>Trả về tên hiển thị kèm mapID trong ngoặc.</summary>
        public static string GetMapDisplayName(int mapID)
        {
            string name = (mapID >= 0 && mapID < MapNames.Length) ? MapNames[mapID] : TileMap.mapNames[mapID];
            if (mapID == 113) return $"Siêu hạng\n[{mapID}]";
            if (mapID == 129) return $"{name} 23\n[{mapID}]";
            return $"{name}\n[{mapID}]";
        }

        // ─────────────────────────────────────────────
        //  KHỞI TẠO ĐỒ THỊ LIÊN KẾT BẢN ĐỒ
        // ─────────────────────────────────────────────

        /// <summary>
        /// Thêm liên kết dạng dây chuyền: mỗi phần tử kề với phần tử liền trước và liền sau.
        /// Ví dụ: AddLink(0, 21) → 0↔21; AddLink(3, 27,28,29,30) → 3↔27↔28↔29↔30
        /// </summary>
        private static void AddLink(params int[] chain)
        {
            for (int i = 0; i < chain.Length; i++)
            {
                if (!linkMaps.ContainsKey(chain[i]))
                    linkMaps.Add(chain[i], new List<NextMap>());
                if (i > 0)
                    linkMaps[chain[i]].Add(new NextMap(chain[i - 1], -1, -1));
                if (i < chain.Length - 1)
                    linkMaps[chain[i]].Add(new NextMap(chain[i + 1], -1, -1));
            }
        }

        /// <summary>Thêm liên kết qua NPC: đứng ở currentMap, nói chuyện NPC npcID, chọn option select.</summary>
        private static void AddNPCLink(int currentMap, int nextMap, int npcID, int select)
        {
            if (!linkMaps.ContainsKey(currentMap))
                linkMaps.Add(currentMap, new List<NextMap>());
            linkMaps[currentMap].Add(new NextMap(nextMap, npcID, select));
        }

        private static void LoadLinkMapsXmap()
        {
            // ── TRÁI ĐẤT – Chuỗi làng chính ──
            AddLink(0, 21);                         // Làng Aru ↔ Nhà Gôhan
            AddLink(6, 47);                         // Đông Karin ↔ Rừng Karin (Cổng chính ra vào Rừng Karin)
            AddLink(47, 111);                       // Rừng Karin ↔ Đông Nam Karin
            AddLink(2, 24);                         // Thung lũng tre ↔ Trạm vũ trụ TĐ
            AddLink(5, 29);                         // Đảo Kamê ↔ Nam Kamê
            AddLink(42, 0, 1, 2, 3, 4, 5, 6);      // Cổng TĐ ↔ tất cả làng TĐ

            // Dungeon Trái Đất (vùng xa Làng Aru)
            AddLink(3, 27, 28, 29, 30);             // Rừng nấm ↔ chuỗi dungeon TĐ

            // Tháp Karin – Thần điện – Kaio
            AddLink(47, 46, 45);                    // Rừng Karin ↔ Tháp Karin ↔ Thần điện
            AddLink(45, 46, 19);                    // Thần điện ↔ Tháp ↔ Thành phố Vegeta (Xayda)

            // Tường thành (Fide xâm lăng)
            AddLink(53, 58, 59, 60, 61, 62);        // Tường thành chuỗi
            AddLink(53, 55, 56, 54, 57);            // Các tầng

            // ── NAMẾC – Chuỗi làng chính ──
            AddLink(7, 22);                         // Làng Mori ↔ Nhà Moori
            AddLink(9, 25);                         // Thị trấn Moori ↔ Trạm vũ trụ Namếc
            AddLink(13, 33);                        // Đảo Guru ↔ Nam Guru
            AddLink(43, 7, 8, 9, 11, 12, 13, 10);  // Cổng Namếc ↔ tất cả làng Namếc
            AddLink(11, 31, 32, 33, 34);            // Thung lũng Maima ↔ chuỗi dungeon Namếc

            // ── XAYDA – Chuỗi làng chính ──
            AddLink(14, 23);                        // Làng Kakarot ↔ Nhà Broly
            AddLink(16, 26);                        // Làng Plant ↔ Trạm vũ trụ Xayda
            AddLink(20, 37);                        // Vách núi đen ↔ Thung lũng đen
            AddLink(52, 44, 14, 15, 16, 17, 18, 20, 19); // Đại hội ↔ Cổng Xayda ↔ làng Xayda
            AddLink(17, 35, 36, 37, 38);            // Rừng nguyên sinh ↔ chuỗi dungeon Xayda

            // Đại hội → Siêu hạng → Freeza
            AddLink(52, 127, 129);                  // Đại hội ↔ Freeza city ↔ Đảo băng
            AddLink(131, 132, 133);                 // Chuỗi hang khỉ (Nappa phụ)

            // ── FIDE (NAPPA) – Chuỗi vùng ──
            AddLink(39, 21);                        // Vách núi Aru  ↔ Nhà Gôhan
            AddLink(40, 22);                        // Vách núi Moori↔ Nhà Moori
            AddLink(41, 23);                        // Vực Plant     ↔ Nhà Broly
            AddLink(68, 69, 70, 71, 72,
                    64, 65, 63, 66, 67,
                    73, 74, 75, 76, 77,
                    81, 82, 83, 79, 80);            // Chuỗi vùng Nappa/Fide chính
            AddLink(80, 131);                       // Núi khỉ vàng ↔ Rừng khỉ

            // ── TƯƠNG LAI ──
            AddLink(102, 92, 93, 94, 96,
                    97, 98, 99, 100, 103);          // Nhà Bunma ↔ chuỗi Tương Lai

            // ── COLD (BĂNG) ──
            AddLink(109, 105, 106, 107, 108, 110); // Rừng băng ↔ chuỗi Cold
            AddLink(109, 108, 107, 110, 106);       // Liên kết thêm bên trong
            AddLink(108, 105);
            AddLink(80, 105);                      // Núi khỉ vàng → Cánh đồng tuyết

            // ── KARIN VÙNG PHỤ ──
            AddLink(47, 46, 45, 48);               // Rừng Karin ↔ Tháp ↔ Thần điện ↔ Kaio
        }

        private static void LoadNPCLinkMapsXmap()
        {
            // Map nguồn  | Map đích | NPC ID | Option
            // ── Thành phố Vegeta (19) ──
            AddNPCLink(19, 68, 12, 1);  // Vegeta → Thung lũng Nappa
            AddNPCLink(19, 109, 12, 0);  // Vegeta → Rừng băng (Cold)

            // ── Trạm vũ trụ ──
            AddNPCLink(24, 25, 10, 0);  // Trạm TĐ → Trạm Namếc  (option 0)
            AddNPCLink(24, 26, 10, 1);  // Trạm TĐ → Trạm Xayda  (option 1)
            AddNPCLink(24, 84, 10, 2);  // Trạm TĐ → Siêu Thị     (option 2)
            AddNPCLink(25, 24, 11, 0);  // Trạm Namếc → Trạm TĐ
            AddNPCLink(25, 26, 11, 1);  // Trạm Namếc → Trạm Xayda
            AddNPCLink(25, 84, 11, 2);  // Trạm Namếc → Siêu Thị
            AddNPCLink(26, 24, 12, 0);  // Trạm Xayda → Trạm TĐ
            AddNPCLink(26, 25, 12, 1);  // Trạm Xayda → Trạm Namếc
            AddNPCLink(26, 84, 12, 2);  // Trạm Xayda → Siêu Thị

            // ── Cổng Tương Lai (map 27/28/29 ↔ 102) qua NPC 38 ──
            AddNPCLink(27, 102, 38, 1);
            AddNPCLink(28, 102, 38, 1);
            AddNPCLink(29, 102, 38, 1);
            AddNPCLink(102, 27, 38, 1);  // Nhà Bunma → Rừng Bamboo

            // ── Tường thành → Trạm ──
            AddNPCLink(27, 53, 25, 0);  // Rừng Bamboo → Tường thành 1

            // ── Tháp Karin / Thần điện ──
            AddNPCLink(45, 46, 19, 3);  // Thần điện ↔ Tháp Karin (NPC 19)

            // ── Đại hội → các map đặc biệt ──
            AddNPCLink(52, 127, 44, 0);  // Đại hội → Freeza city
            AddNPCLink(52, 129, 23, 3);  // Đại hội → Đảo băng giá
            AddNPCLink(52, 113, 23, 2);  // Đại hội → Siêu hạng
            AddNPCLink(113, 52, 22, 4);  // Siêu hạng → Đại hội
            AddNPCLink(127, 52, 44, 2);  // Freeza city → Đại hội
            AddNPCLink(129, 52, 23, 3);  // Đảo băng → Đại hội

            // ── Núi khỉ vàng ↔ Rừng khỉ ──
            AddNPCLink(80, 131, 60, 0); // Núi khỉ vàng → Rừng khỉ
            AddNPCLink(131, 80, 60, 1); // Rừng khỉ → Núi khỉ vàng

            // ── Thung lũng Nappa ↔ Thành phố Vegeta ──
            AddNPCLink(68, 19, 12, 0);
        }

        private static void AddPlanetXmap()
        {
            planetDictionary.Add("Trái Đất", idMapsTraiDat);
            planetDictionary.Add("Namếc", idMapsNamek);
            planetDictionary.Add("Xayda", idMapsXayda);
            planetDictionary.Add("Fide", idMapsNappa);
            planetDictionary.Add("Tương Lai", idMapsTuongLai);
            planetDictionary.Add("Cold", idMapsCold);
            planetDictionary.Add("Hải Tặc", idMapsHaiTac);
            planetDictionary.Add("Potaufeu", idMapsPotaufeu);
            planetDictionary.Add("Nguyên Thủy", idMapsNguyenThuy);
            planetDictionary.Add("Ngục Tù", idMapsNguTu);
            planetDictionary.Add("Bill", idMapsBill);
            planetDictionary.Add("Thánh Địa", idMapsThanhDia);
            planetDictionary.Add("Cold 2", idMapsCold2);
            planetDictionary.Add("Khác", idMapsKhac);
        }

        // ─────────────────────────────────────────────
        //  LƯU / ĐỌC DỮ LIỆU
        // ─────────────────────────────────────────────

        private static void LoadData()
        {
            isSaveData = (Rms.loadRMSInt("AutoMapIsSaveRms") == 1);
            if (!isSaveData) return;

            isEatChicken = Rms.loadRMSInt("AutoMapIsEatChicken") != 0;  // mặc định ON nếu -1
            isUseCapsule = Rms.loadRMSInt("AutoMapIsUseCsb") != 0;
            isHarvestPean = Rms.loadRMSInt("AutoMapIsHarvestPean") == 1;
        }

        private static void SaveData()
        {
            Rms.saveRMSInt("AutoMapIsEatChicken", isEatChicken ? 1 : 0);
            Rms.saveRMSInt("AutoMapIsHarvestPean", isHarvestPean ? 1 : 0);
            Rms.saveRMSInt("AutoMapIsUseCsb", isUseCapsule ? 1 : 0);
        }

        // ─────────────────────────────────────────────
        //  CLASS NỘI BỘ: NextMap – một bước di chuyển
        // ─────────────────────────────────────────────
        public class NextMap
        {
            public int MapID;   // MapID đích sẽ đến
            public int Npc;     // NPC ID cần nói chuyện (-1 = dùng waypoint)
            public int Index;   // Option trong menu NPC (-1 = dùng waypoint)

            public NextMap(int mapID, int npcID, int index)
            {
                MapID = mapID;
                Npc = npcID;
                Index = index;
            }

            /// <summary>Thực hiện di chuyển: qua waypoint hoặc nói chuyện NPC.</summary>
            public void GotoMap()
            {
                if (Npc == -1 && Index == -1)
                {
                    // Di chuyển qua cổng waypoint
                    Waypoint wp = GetWayPoint();
                    if (wp != null) Enter(wp);
                }
                else if (Npc != -1 && Index != -1)
                {
                    // Di chuyển qua NPC
                    Npc targetNpc = GameScr.findNPCInMap((short)Npc);
                    if (targetNpc != null)
                    {
                        TeleportTo(targetNpc.cx, targetNpc.cy - 3);
                    }
                    Service.gI().openMenu(Npc);
                    Service.gI().confirmMenu((short)Npc, (sbyte)Index);
                }
            }

            private static readonly string[] VietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            public static string RemoveVietnameseSigns(string text)
            {
                if (string.IsNullOrEmpty(text)) return string.Empty;
                string clean = text.Trim().ToLower();
                for (int i = 1; i < VietnameseSigns.Length; i++)
                {
                    for (int j = 0; j < VietnameseSigns[i].Length; j++)
                        clean = clean.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
                return clean;
            }

            /// <summary>Tìm waypoint trong map hiện tại có tên khớp với MapID đích.</summary>
            public Waypoint GetWayPoint()
            {
                string targetName = GetMapName();
                if (string.IsNullOrEmpty(targetName) && MapID >= 0 && MapID < MapNames.Length)
                {
                    targetName = MapNames[MapID];
                }
                if (string.IsNullOrEmpty(targetName) || TileMap.vGo == null) return null;
                string cleanTarget = RemoveVietnameseSigns(targetName);

                for (int i = 0; i < TileMap.vGo.size(); i++)
                {
                    if (i >= TileMap.vGo.size()) break;
                    Waypoint wp = (Waypoint)TileMap.vGo.elementAt(i);
                    if (wp == null || wp.popup == null) continue;
                    string wpName = GetMapName(wp.popup);
                    if (string.IsNullOrEmpty(wpName)) continue;

                    if (targetName.Equals(wpName, StringComparison.OrdinalIgnoreCase)
                        || wpName.IndexOf(targetName, StringComparison.OrdinalIgnoreCase) >= 0
                        || targetName.IndexOf(wpName, StringComparison.OrdinalIgnoreCase) >= 0)
                        return wp;

                    string cleanWp = RemoveVietnameseSigns(wpName);
                    if (cleanTarget.Equals(cleanWp, StringComparison.OrdinalIgnoreCase)
                        || cleanWp.IndexOf(cleanTarget, StringComparison.OrdinalIgnoreCase) >= 0
                        || cleanTarget.IndexOf(cleanWp, StringComparison.OrdinalIgnoreCase) >= 0)
                        return wp;
                }
                return null;
            }

            public string GetMapName()
            {
                if (TileMap.mapNames != null && MapID >= 0 && MapID < TileMap.mapNames.Length && !string.IsNullOrEmpty(TileMap.mapNames[MapID]))
                    return TileMap.mapNames[MapID];
                if (MapID >= 0 && MapID < MapNames.Length)
                    return MapNames[MapID];
                return string.Empty;
            }

            public string GetMapName(PopUp popup)
            {
                if (popup == null || popup.says == null) return string.Empty;
                var sb = new StringBuilder();
                foreach (string s in popup.says)
                {
                    if (!string.IsNullOrEmpty(s)) sb.Append(s).Append(' ');
                }
                return sb.ToString().Trim();
            }

            /// <summary>Tính tọa độ đứng trước waypoint rồi teleport + đổi map.</summary>
            public void Enter(Waypoint wp)
            {
                if (wp == null) return;
                int x = (wp.maxX < 60)
                        ? 15
                        : ((int)wp.minX > TileMap.pxw - 60
                           ? TileMap.pxw - 15
                           : (int)((wp.minX + wp.maxX) / 2));
                int y = (int)wp.maxY;

                if (x == -1 || y == -1)
                { GameScr.info1.addInfo("Có lỗi xảy ra", 0); return; }

                TeleportTo(x, y);
                if (wp.isOffline) Service.gI().getMapOffline();
                else Service.gI().requestChangeMap();
            }

            public void TeleportTo(int x, int y)
            {
                Char.myCharz().cx = x;
                Char.myCharz().cy = y;
                Service.gI().charMove();
                if (!GameScr.canAutoPlay)
                {
                    Char.myCharz().cx = x;
                    Char.myCharz().cy = y + 1;
                    Service.gI().charMove();
                    Char.myCharz().cx = x;
                    Char.myCharz().cy = y;
                    Service.gI().charMove();
                }
            }
        }
    }
}