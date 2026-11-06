using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;


namespace Mod
{
    public class MainMod:IActionListener
    {
    
        public static MainMod getInstance()
        {
            if (MainMod._Instance == null)
            {
                MainMod._Instance = new MainMod();
            }
            return MainMod._Instance;
        }
        public static bool Chat(string text)
        {
            if (text.Equals(""))
            {
                return false;
            }
            else if (text.StartsWith("k_"))
            {
                try
                {
                    int zoneId = int.Parse(text.Split(new char[]
                    {
                        '_'
                    })[1]);
                    Service.gI().requestChangeZone(zoneId, -1);
                }
                catch
                {
                }
                return true;
            }
            else if (text.StartsWith("s_"))
            {
                try
                {
                    int num = MainMod.runSpeed = int.Parse(text.Split(new char[]
                    {
                        '_'
                    })[1]);
                    GameScr.info1.addInfo("Tốc Độ Di Chuyển: " + num.ToString(), 0);
                }
                catch
                {
                }
                return true;
            }
            else if (text.StartsWith("cheat_"))
            {
                try
                {
                    float num4 = Time.timeScale = float.Parse(text.Split(new char[]
                    {
                        '_'
                    })[1]);
                    GameScr.info1.addInfo("Cheat: " + num4.ToString(), 0);
                }
                catch
                {
                }
                return true;
            }
            else if (text.StartsWith("cheatf_"))
            {
                try
                {
                    float num5 = Time.timeScale = float.Parse(text.Split(new char[]
                    {
                        '_'
                    })[1]) / 10f;
                    GameScr.info1.addInfo("Cheat: " + num5.ToString(), 0);
                }
                catch
                {
                }
                return true;
            }
            else if (text.StartsWith("speed_"))
            {
                try
                {
                    runSpeed = int.Parse(text.Split('_')[1]);
                    GameScr.info1.addInfo("Tốc độ chạy: " + runSpeed, 0);
                }
                catch { }
                return true;
            }
            else
            {
                return false;
            }
        }
        private static int lastTimeSec = -1;
        private static string cachedTimeText = "";
        public static void paint(mGraphics g)
        {
            
            try
            {
                paintLogo(g);
                paintFPS(g);
                paintHP_MP(g);
                paintTime(g);
                if (ttdt)
                {
                    ShowInfor(g);
                }
                int num = GameCanvas.h - 220;
                if (AutoSkill.isAutoSendAttack)
                {
                    DrawTextWithBorder(g, mFont.tahoma_7_yellow, "Tự đánh: Bật", 10, num, 0);
                    num += 10;
                }
                if (MainMod.isAutoRevive)
                {
                    DrawTextWithBorder(g, mFont.tahoma_7_yellow, "Hồi sinh: Bật", 10, num, 0);
                    num += 10;
                }
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi MainMod.paint: " + ex);
            }
        }
        public static void ShowInfor(mGraphics g)
        {
            g.setColor(Color.yellow);
            int num = (int)((double)global::Char.myPetz().cHP / (double)global::Char.myPetz().cHPFull * 100.0);
            int num2 = (int)((double)global::Char.myPetz().cMP / (double)global::Char.myPetz().cMPFull * 100.0);
            int num3 = (int)((double)global::Char.myPetz().cStamina / (double)global::Char.myPetz().cMaxStamina * 100.0);
            int num4 = (int)((double)global::Char.myCharz().cHP / (double)global::Char.myCharz().cHPFull * 100.0);
            int num5 = (int)((double)global::Char.myCharz().cMP / (double)global::Char.myCharz().cMPFull * 100.0);
            int num6 = (int)((double)global::Char.myCharz().cStamina / (double)global::Char.myCharz().cMaxStamina * 100.0);
            int num7 = GameCanvas.h - 230;
            if (ttdt)
            {
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, "__Đệ Tử__", 10, num7 + 20, 0);
                DrawTextWithBorder(g,mFont.tahoma_7_yellow, "Sức Mạnh: " + global::Char.myPetz().cPower.ToString("#,##0"), 10, num7 + 30, 0);
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, "Tiềm Năng: " + global::Char.myPetz().cTiemNang.ToString("#,##0"), 10, num7 + 40, 0);
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, string.Concat(new string[]
                {
                    "HP: ",
                    global::Char.myPetz().cHP.ToString("#,##0"),
                    "(",
                    num.ToString(),
                    "%)"
                }), 10, num7 + 50, 0);
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, string.Concat(new string[]
                {
                    "KI: ",
                    global::Char.myPetz().cMP.ToString("#,##0"),
                    "(",
                    num2.ToString(),
                    "%)"
                }), 10, num7 + 60, 0);
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, "Sức Đánh: " + global::Char.myPetz().cDamFull.ToString("#,##0"), 10, num7 + 70, 0);
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, "Giáp: " + global::Char.myPetz().cDefull.ToString("#,##0"), 10, num7 + 80, 0);
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, string.Concat(new string[]
                {
                    "Thể Lực: ",
                    global::Char.myPetz().cStamina.ToString("#,##0"),
                    "(",
                    num3.ToString(),
                    "%)"
                }), 10, num7 + 90, 0);
            }
        }
        public static void paintTime(mGraphics g)
        {
            DateTime now = DateTime.Now;
            if (now.Second != lastTimeSec || string.IsNullOrEmpty(cachedTimeText))
            {
                lastTimeSec = now.Second;
                cachedTimeText = now.ToString("HH:mm:ss dd/MM/yyyy");
            }
            DrawTextWithBorder(g, mFont.tahoma_7_yellow, cachedTimeText, 5, GameCanvas.h - 11, 0);
        }
        private static int lastRenderedFps = -1;
        private static int lastRenderedPing = -1;
        private static string cachedFpsText = "FPS: 60 - 0 ms";
        public static bool isShowHP_MP = true;
        public static bool isShowFPS = true;
        public static int currentFps = 60;
        private static int fpsCounter = 0;
        private static long lastFpsUpdate = 0;
        public static int currentPing = 0;
        private static long lastPingCheck = 0;
        private static bool isAttemptedLoadStar = false;
        public static bool isGoBack;
        private static bool isPinging = false;
        private static readonly string[] staticStarSlotStrings = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private static readonly string[] staticStarSlotStarStrings = new string[] { "0 ★", "1 ★", "2 ★", "3 ★", "4 ★", "5 ★", "6 ★", "7 ★", "8 ★", "9 ★" };
        public static DateTime onlineStartTime = DateTime.MinValue;

        private static int lastOnlineSec = -1;
        private static string cachedOnlineTimeStr = "0p0s";

        public static string GetOnlineTimeString()
        {
            if (onlineStartTime == DateTime.MinValue)
            {
                if (global::Char.myCharz() != null)
                {
                    onlineStartTime = DateTime.Now;
                }
                else
                {
                    return "0p0s";
                }
            }

            int totalSec = (int)(DateTime.Now - onlineStartTime).TotalSeconds;
            if (totalSec < 0) totalSec = 0;

            if (totalSec != lastOnlineSec)
            {
                lastOnlineSec = totalSec;
                int hours = totalSec / 3600;
                int minutes = (totalSec % 3600) / 60;
                int seconds = totalSec % 60;

                if (hours > 0)
                {
                    cachedOnlineTimeStr = hours + "h" + minutes + "p" + seconds + "s";
                }
                else
                {
                    cachedOnlineTimeStr = minutes + "p" + seconds + "s";
                }
            }
            return cachedOnlineTimeStr;
        }
        public static int GetClosestGroundY(int x, int targetY)
        {
            List<int> list = new List<int>();
            int num = 24;
            int i = 50;
            for (int mapEndY = TileMap.GetMapEndY(); i <= mapEndY; i += num)
            {
                if (TileMap.tileTypeAt(x, i, 2))
                {
                    if (i % 24 != 0)
                    {
                        i -= i % 24;
                    }
                    list.Add(i);
                }
            }
            if (list.Count == 0)
            {
                GameScr.info1.addInfo($"Không tìm thấy mặt đất tại x={x}, sử dụng y={AutoMap.GetYGround(x)}",0);
                return AutoMap.GetYGround(x);
            }
            int result = list[0];
            int num2 = Math.abs(list[0] - targetY);
            foreach (int item in list)
            {
                int num3 = Math.abs(item - targetY);
                if (num3 < num2)
                {
                    num2 = num3;
                    result = item;
                }
            }
            return result;
        }
        public static bool tieuDietNguoiBatCo;
        public static bool isBoss(Char ch)
        {
            return ch.cName != null && ch.cName != "" && !ch.isPet && !ch.isMiniPet && char.IsUpper(char.Parse(ch.cName.Substring(0, 1))) && ch.cName != "Trọng tài" && !ch.cName.StartsWith("#") && !ch.cName.StartsWith("$");
        }
        public static bool CheckBossMob(int idBossMod)
        {
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (!mob.isMobMe && mob.templateId == idBossMod && mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isDie && mob.x > 0 && mob.xFirst > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static int GetItemStarSlot(Item item)
        {
            if (item == null || item.itemOption == null)
            {
                return 0;
            }
            if (item.cachedStarSlot != -1)
            {
                return item.cachedStarSlot;
            }
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                if (item.itemOption[i] != null && item.itemOption[i].optionTemplate != null && item.itemOption[i].optionTemplate.id == 102)
                {
                    item.cachedStarSlot = item.itemOption[i].param;
                    return item.cachedStarSlot;
                }
            }
            item.cachedStarSlot = 0;
            return 0;
        }

        public static string GetItemSPLText(Item item)
        {
            if (item == null || item.itemOption == null)
            {
                return string.Empty;
            }
            if (item.cachedSPLText != null)
            {
                return item.cachedSPLText;
            }

            string result = string.Empty;
            int itemType = (item.template != null) ? item.template.type : -1;

            for (int i = 0; i < item.itemOption.Length; i++)
            {
                ItemOption opt = item.itemOption[i];
                if (opt == null || opt.optionTemplate == null) continue;

                int id = opt.optionTemplate.id;
                int param = opt.param;

                // Skip metadata / system options
                if (id == 102 || id == 107 || id == 72 || id == 41 || id == 228 || id == 210 || id == 211 || id == 212)
                {
                    continue;
                }

                // Skip base stat of equipment
                if (i == 0)
                {
                    continue;
                }
                if (itemType == 0 && id == 47) continue; // Giáp on Áo
                if (itemType == 1 && (id == 6 || id == 22)) continue; // HP on Quần
                if (itemType == 2 && id == 0) continue; // Tấn công on Găng
                if (itemType == 3 && (id == 7 || id == 23)) continue; // KI on Giày
                if (itemType == 4 && id == 14) continue; // Chí mạng on Rada
                if (itemType == 32 && id == 9) continue; // Hiệu lực on Giáp tập luyện

                string shortOpt = string.Empty;
                if (id == 100)
                {
                    shortOpt = param + "% COIN";
                }
                else if (id == 101)
                {
                    shortOpt = param + "% TNSM";
                }
                else if (id == 95 || id == 104)
                {
                    shortOpt = param + "% Hút HP";
                }
                else if (id == 96)
                {
                    shortOpt = param + "% Hút KI";
                }
                else if (id == 97)
                {
                    shortOpt = param + "% Phản ST";
                }
                else if (id == 94)
                {
                    shortOpt = param + "% GST";
                }
                else if (id == 108)
                {
                    shortOpt = param + "% Né";
                }
                else if (id == 77)
                {
                    shortOpt = param + "% HP";
                }
                else if (id == 103)
                {
                    shortOpt = param + "% KI";
                }
                else if (id == 147 || id == 50 || id == 49)
                {
                    shortOpt = param + "% SĐ";
                }
                else if (id == 5)
                {
                    shortOpt = param + "% STCM";
                }
                else if (id == 204)
                {
                    shortOpt = param + "% ST Boss";
                }
                else if (id == 10)
                {
                    shortOpt = param + "% STC";
                }
                else
                {
                    string name = opt.optionTemplate.name;
                    string lower = name.ToLower();
                    if (lower.Contains("vàng rơi") || lower.Contains("coin"))
                    {
                        shortOpt = param + "% COIN";
                    }
                    else if (lower.Contains("tiềm năng") || lower.Contains("tnsm"))
                    {
                        shortOpt = param + "% TNSM";
                    }
                    else if (lower.Contains("thành hp") || lower.Contains("hút máu") || lower.Contains("hút hp"))
                    {
                        shortOpt = param + "% Hút HP";
                    }
                    else if (lower.Contains("thành ki") || lower.Contains("thành mp") || lower.Contains("hút ki"))
                    {
                        shortOpt = param + "% Hút KI";
                    }
                    else if (lower.Contains("phản") && lower.Contains("sát thương"))
                    {
                        shortOpt = param + "% Phản ST";
                    }
                    else if (lower.Contains("giảm") && lower.Contains("sát thương"))
                    {
                        shortOpt = param + "% GST";
                    }
                    else if (lower.Contains("né đòn") || lower.Contains("né"))
                    {
                        shortOpt = param + "% Né";
                    }
                    else if (lower.Contains("quái") && (lower.Contains("sức đánh") || lower.Contains("tấn công") || lower.Contains("dã hạ")))
                    {
                        shortOpt = param + "% MS";
                    }
                    else if (lower.Contains("bản thân") || lower.Contains("tháo ra"))
                    {
                        shortOpt = param + "% BS";
                    }
                    else if (itemType == 32)
                    {
                        if (lower.Contains("quái")) shortOpt = param + "% MS";
                        else if (lower.Contains("thân") || lower.Contains("tháo")) shortOpt = param + "% BS";
                    }
                    else if (name.Contains("%"))
                    {
                        string cleanName = NinjaUtil.replace(name, "#", string.Empty).Replace("+", "").Replace("%", "").Trim();
                        shortOpt = param + "% " + cleanName;
                    }
                }

                if (!string.IsNullOrEmpty(shortOpt))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += ", ";
                    }
                    result += shortOpt;
                }
            }

            item.cachedSPLText = result;
            return result;
        }
        public static void PaintItemStarAndSPL(mGraphics g, Item item, int xRight, int yRow)
        {
            if (item == null || item.template == null || item.itemOption == null)
            {
                return;
            }

            int starSlot = GetItemStarSlot(item);
            if (starSlot <= 0)
            {
                return;
            }

            if (!isAttemptedLoadStar && Panel.imgStar == null)
            {
                isAttemptedLoadStar = true;
                try
                {
                    Panel.imgStar = GameCanvas.loadImage("/mainImage/star.png");
                }
                catch (Exception)
                {
                }
            }

            string slotStr = (starSlot >= 0 && starSlot < staticStarSlotStrings.Length) ? staticStarSlotStrings[starSlot] : starSlot.ToString();

            // Dòng 1: Số sao + Icon sao pha lê xanh
            if (Panel.imgStar != null)
            {
                int starW = mGraphics.getImageWidth(Panel.imgStar);
                int starH = mGraphics.getImageHeight(Panel.imgStar);
                int fontH = mFont.tahoma_7_yellow.getHeight();
                int starY = yRow + 1 + ((fontH > starH) ? ((fontH - starH) / 2) : 0);
                g.drawImage(Panel.imgStar, xRight, starY, mGraphics.TOP | mGraphics.RIGHT);
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, slotStr, xRight - starW - 2, yRow + 1, mFont.RIGHT);
            }
            else
            {
                string slotStarStr = (starSlot >= 0 && starSlot < staticStarSlotStarStrings.Length) ? staticStarSlotStarStrings[starSlot] : (starSlot + " ★");
                DrawTextWithBorder(g,mFont.tahoma_7_yellow, slotStarStr, xRight, yRow + 1, mFont.RIGHT);
            }
         
        }


        public static void paintFPS(mGraphics g)
        {
            if (!isShowFPS)
            {
                return;
            }

            // Đo FPS thực tế mỗi 500ms
            fpsCounter++;
            long now = mSystem.currentTimeMillis();
            if (now - lastFpsUpdate >= 500)
            {
                if (lastFpsUpdate > 0)
                {
                    currentFps = (int)(fpsCounter * 1000L / (now - lastFpsUpdate));
                }
                fpsCounter = 0;
                lastFpsUpdate = now;
            }

            try
            {
                GameScr.resetTranslate(g);

                int pingVal = (currentPing > 0) ? currentPing : 0;
                if (currentFps != lastRenderedFps || pingVal != lastRenderedPing || cachedFpsText == null)
                {
                    lastRenderedFps = currentFps;
                    lastRenderedPing = pingVal;
                    cachedFpsText = "FPS: " + currentFps + " - " + pingVal + " ms";
                }

                int x = 83;
                int y = 30;

                // Hiển thị màu đỏ có viền đen sắc nét
                DrawTextWithBorder(g, mFont.tahoma_7_red, cachedFpsText, x, y,0);
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi paintFPS: " + ex);
            }
        }

        private static string lastWorkingPingHost = null;
        private static int lastWorkingPingPort = 14445;

        /// <summary>
        /// Đo độ trễ kết nối TCP (Round-trip time / Ping ms) thực tế, không cần quyền Admin và không bị chặn ICMP
        /// </summary>
        private static int MeasureTcpPing(string host, int port, int timeoutMs)
        {
            Socket socket = null;
            try
            {
                if (string.IsNullOrEmpty(host))
                {
                    return -1;
                }

                Stopwatch sw = new Stopwatch();
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Blocking = false;
                sw.Start();
                IAsyncResult asyncResult = socket.BeginConnect(host, port, null, null);
                bool completed = asyncResult.AsyncWaitHandle.WaitOne(timeoutMs, true);
                try
                {
                    if (completed && socket.Connected)
                    {
                        sw.Stop();
                        socket.EndConnect(asyncResult);
                        return (int)sw.ElapsedMilliseconds;
                    }
                }
                finally
                {
                    try
                    {
                        if (asyncResult != null && asyncResult.AsyncWaitHandle != null)
                        {
                            asyncResult.AsyncWaitHandle.Close();
                        }
                    }
                    catch { }
                }
            }
            catch
            {
            }
            finally
            {
                if (socket != null)
                {
                    try
                    {
                        socket.Close();
                    }
                    catch
                    {
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Đo Ping tới server game hoặc Google/Cloudflare DNS trên background thread
        /// </summary>
        public static void UpdatePing()
        {
            long now = mSystem.currentTimeMillis();
            if (now - lastPingCheck < 5000)
            {
                return;
            }

            if (isPinging)
            {
                // Timeout phòng trường hợp thread trước bị treo quá 4 giây
                if (now - lastPingCheck > 4000)
                {
                    isPinging = false;
                }
                return;
            }

            lastPingCheck = now;
            isPinging = true;

            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    int ms = -1;

                    // 1. Thử host đã hoạt động lần trước trước tiên để không phải scan tuần tự nhiều host
                    if (!string.IsNullOrEmpty(lastWorkingPingHost))
                    {
                        ms = MeasureTcpPing(lastWorkingPingHost, lastWorkingPingPort, 500);
                    }

                    // 2. Nếu chưa có hoặc host cũ timeout, đo server game hiện tại
                    if (ms < 0)
                    {
                        string host = GameMidlet.IP;
                        int port = (GameMidlet.PORT > 0) ? GameMidlet.PORT : 14445;
                        if (!string.IsNullOrEmpty(host))
                        {
                            ms = MeasureTcpPing(host, port, 500);
                            if (ms >= 0)
                            {
                                lastWorkingPingHost = host;
                                lastWorkingPingPort = port;
                            }
                        }
                    }

                    // 3. Fallback server mặc định
                    if (ms < 0)
                    {
                        ms = MeasureTcpPing("112.213.94.23", 14445, 500);
                        if (ms >= 0)
                        {
                            lastWorkingPingHost = "112.213.94.23";
                            lastWorkingPingPort = 14445;
                        }
                    }

                    // 4. Fallback Google DNS
                    if (ms < 0)
                    {
                        ms = MeasureTcpPing("8.8.8.8", 53, 500);
                        if (ms >= 0)
                        {
                            lastWorkingPingHost = "8.8.8.8";
                            lastWorkingPingPort = 53;
                        }
                    }

                    // 5. Fallback Cloudflare DNS
                    if (ms < 0)
                    {
                        ms = MeasureTcpPing("1.1.1.1", 53, 500);
                        if (ms >= 0)
                        {
                            lastWorkingPingHost = "1.1.1.1";
                            lastWorkingPingPort = 53;
                        }
                    }

                    if (ms >= 0)
                    {
                        // Giảm jitter (dao động nhẹ) để số ping hiển thị mượt mà và chính xác
                        if (currentPing <= 0)
                        {
                            currentPing = ms;
                        }
                        else
                        {
                            currentPing = (int)(currentPing * 0.35f + ms * 0.65f);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    isPinging = false;
                }
            });
        }

        public static Texture2D logoGameTexture = null;
        private static bool isAttemptedLoadLogo = false;
        private static long lastTryLoadLogo = 0;
        public static bool isShowLogo = true;
        public static float logoWidth = 260f;
        public static float logoHeight = 0f;
        public static float logoX = -1f;
        public static float logoY = 0f;
        private static int logoLoadAttempts = 0;
        // ===== Cần có các using sau ở đầu file =====
        // using System;
        // using System.Text;
        // using UnityEngine;

        // ===================== CẤU HÌNH =====================
        private const string ASSET_ROOT = @"E:\mod\DragonBoy250_pc\DragonBoy250_Data";

        private static readonly byte[] CryptoKey = Encoding.UTF8.GetBytes("NRO_TIGO_SECURE_KEY_2026_MOD!@#$"); // 32 byte
        private static readonly byte[] CryptoIV = Encoding.UTF8.GetBytes("NRO_TIGO_IV_2026");                  // 16 byte
        private static readonly byte[] CryptoMagic = new byte[] { 0x4D, 0x4F, 0x44, 0x54 };                   // "MODT"
        private const byte CryptoXor = 0x5A;

        // ===================== LOGO =====================
        public static void LoadLogoTexture()
        {
            try
            {
                byte[] data = LoadDecryptedAsset("logo.dat", "LOGOGAME.png");
                if (data != null && data.Length > 0)
                {
                    Texture2D tex = new Texture2D(2, 2);
                    if (tex.LoadImage(data))
                    {
                        tex.filterMode = FilterMode.Bilinear;
                        tex.wrapMode = TextureWrapMode.Clamp;
                        logoGameTexture = tex;
                    }
                }
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi LoadLogoTexture: " + ex);
            }
        }

        public static void paintLogo(mGraphics g)
        {
            if (!isShowLogo)
            {
                return;
            }

            try
            {
                long now = mSystem.currentTimeMillis();

                // Thử load lại tối đa 5 lần, cách nhau 5 giây
                if (logoGameTexture == null && logoLoadAttempts < 5 &&
                    (!isAttemptedLoadLogo || now - lastTryLoadLogo > 5000))
                {
                    logoLoadAttempts++;
                    isAttemptedLoadLogo = true;
                    lastTryLoadLogo = now;
                    LoadLogoTexture();
                }

                if (logoGameTexture != null)
                {
                    GameScr.resetTranslate(g);

                    float screenW = (float)Screen.width;
                    float drawW = logoWidth;

                    if (drawW <= 0f)
                    {
                        drawW = 260f;
                    }
                    if (screenW > 0f)
                    {
                        if (screenW <= 800f)
                        {
                            drawW = Mathf.Clamp(screenW * 0.28f, 180f, 240f);
                        }
                        else if (screenW >= 1200f)
                        {
                            drawW = Mathf.Clamp(screenW * 0.25f, 260f, 320f);
                        }
                    }

                    float drawH = logoHeight;
                    if (drawH <= 0f && logoGameTexture.width > 0)
                    {
                        drawH = drawW * ((float)logoGameTexture.height / (float)logoGameTexture.width);
                    }

                    float drawX = (logoX >= 0f) ? logoX : ((screenW - drawW) / 2f);
                    float drawY = logoY;

                    Color oldColor = GUI.color;
                    GUI.color = Color.white;
                    GUI.DrawTexture(new Rect(drawX, drawY, drawW, drawH), logoGameTexture, ScaleMode.StretchToFill, true);
                    GUI.color = oldColor;
                }
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi paintLogo: " + ex);
            }
        }

        // ===================== TÌM FILE =====================
        private static string[] GetSearchDirs()
        {
            return new string[]
            {
        System.IO.Path.Combine(ASSET_ROOT, "Data"),   // ...\DragonBoy250_Data\Data
        ASSET_ROOT,                                   // ...\DragonBoy250_Data
        System.IO.Path.Combine(Application.dataPath, "Data"),
        Application.dataPath,
        Environment.CurrentDirectory
            };
        }

        public static byte[] LoadDecryptedAsset(string datName, string fallbackFileName)
        {
            try
            {
                // 1. Embedded Resource trong Assembly-CSharp.dll
                try
                {
                    System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                    string[] resNames = asm.GetManifestResourceNames();
                    if (resNames != null)
                    {
                        for (int i = 0; i < resNames.Length; i++)
                        {
                            if (!resNames[i].EndsWith(datName, StringComparison.OrdinalIgnoreCase)) continue;
                            using (System.IO.Stream stream = asm.GetManifestResourceStream(resNames[i]))
                            {
                                if (stream == null) continue;
                                byte[] buffer = new byte[stream.Length];
                                int read = 0;
                                while (read < buffer.Length)
                                {
                                    int n = stream.Read(buffer, read, buffer.Length - read);
                                    if (n <= 0) break;
                                    read += n;
                                }
                                byte[] dec = DecryptAsset(buffer);
                                if (dec != null && dec.Length > 0) return dec;
                            }
                        }
                    }
                }
                catch { }

                string[] dirs = GetSearchDirs();

                // 2. File .dat đã mã hóa trên ổ đĩa
                for (int i = 0; i < dirs.Length; i++)
                {
                    try
                    {
                        string p = System.IO.Path.Combine(dirs[i], datName);
                        if (System.IO.File.Exists(p))
                        {
                            byte[] dec = DecryptAsset(System.IO.File.ReadAllBytes(p));
                            if (dec != null && dec.Length > 0) return dec;
                        }
                    }
                    catch { }
                }

                // 3. Fallback: ảnh gốc chưa mã hóa
                if (!string.IsNullOrEmpty(fallbackFileName))
                {
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        try
                        {
                            string p = System.IO.Path.Combine(dirs[i], fallbackFileName);
                            if (System.IO.File.Exists(p))
                            {
                                byte[] raw = System.IO.File.ReadAllBytes(p);
                                if (raw != null && raw.Length > 0) return raw;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi LoadDecryptedAsset " + datName + ": " + ex);
            }
            return null;
        }

        // ===================== GIẢI MÃ =====================
        public static byte[] DecryptAsset(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                return null;
            }

            // PNG hoặc JPEG nguyên bản thì trả luôn
            if (input.Length >= 4)
            {
                if (input[0] == 0x89 && input[1] == 0x50 && input[2] == 0x4E && input[3] == 0x47) return input;
                if (input[0] == 0xFF && input[1] == 0xD8 && input[2] == 0xFF) return input;
            }

            // Kiểm tra magic header "MODT"
            if (input.Length <= CryptoMagic.Length)
            {
                return null;
            }
            for (int i = 0; i < CryptoMagic.Length; i++)
            {
                if (input[i] != CryptoMagic[i])
                {
                    return null;
                }
            }

            // Bỏ XOR 0x5A
            int cipherLen = input.Length - CryptoMagic.Length;
            byte[] aesEncrypted = new byte[cipherLen];
            for (int i = 0; i < cipherLen; i++)
            {
                aesEncrypted[i] = (byte)(input[CryptoMagic.Length + i] ^ CryptoXor);
            }

            // AES-256-CBC, PKCS7
            try
            {
                using (System.Security.Cryptography.RijndaelManaged rij = new System.Security.Cryptography.RijndaelManaged())
                {
                    rij.Key = CryptoKey;
                    rij.IV = CryptoIV;
                    rij.Mode = System.Security.Cryptography.CipherMode.CBC;
                    rij.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(
                            ms, rij.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                        {
                            cs.Write(aesEncrypted, 0, aesEncrypted.Length);
                            cs.FlushFinalBlock();
                        }
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi giai ma asset: " + ex);
                return null;
            }
        }

        private static long lastCachedCHP = -1;
        private static long lastCachedCMP = -1;
        private static string cachedStrHP = "";
        private static string cachedStrMP = "";
        public static void paintHP_MP(mGraphics g)
        {
            if (!isShowHP_MP || global::Char.myCharz() == null)
            {
                return;
            }

            try
            {
                GameScr.resetTranslate(g);

                long cHP = global::Char.myCharz().cHP;
                long cMP = global::Char.myCharz().cMP;
                if (cHP < 0) cHP = 0;
                if (cMP < 0) cMP = 0;

                if (cHP != lastCachedCHP || cachedStrHP == null)
                {
                    lastCachedCHP = cHP;
                    cachedStrHP = global::NinjaUtil.getMoneys(cHP);
                }
                if (cMP != lastCachedCMP || cachedStrMP == null)
                {
                    lastCachedCMP = cMP;
                    cachedStrMP = global::NinjaUtil.getMoneys(cMP);
                }

                int hpW = (int)((GameScr.hpBarW > 0) ? GameScr.hpBarW : 66);
                int mpW = (GameScr.mpBarW > 0) ? GameScr.mpBarW : 59;

                int xHp = 83 + hpW / 2;
                int yHp = 4;

                int xMp = 83 + mpW / 2;
                int yMp = 18;

                // 1. Hiển thị HP thực trên thanh máu (Chữ trắng, viền đen mỏng sắc nét)
                DrawTextWithBorder(g, mFont.tahoma_7_white, cachedStrHP, xHp, yHp, mFont.CENTER, Color.white);

                // 2. Hiển thị KI/MP thực trên thanh KI (Chữ vàng, viền đen mỏng sắc nét)
                DrawTextWithBorder(g, mFont.tahoma_7_yellow, cachedStrMP, xMp, yMp, mFont.CENTER, Color.yellow);
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi paintHP_MP: " + ex);
            }
        }
        public static bool PhimTat(int key)
        {
            if (global::Char.myCharz() == null)
            {
                return false;
            }

            int k = (key > 0) ? key : GameCanvas.keyAsciiPress;
            if (k >= 65 && k <= 90)
            {
                k += 32;
            }

            if (k == HotKeys.D)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                Service.gI().getFlag(0, -1);
                return true;
            }
            else if (GameCanvas.keyAsciiPress == HotKeys.A)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                AutoSkill.isAutoSendAttack = !AutoSkill.isAutoSendAttack;
                GameScr.info1.addInfo("Tự Đánh\n" + (AutoSkill.isAutoSendAttack ? "ON" : "OFF"), 0);
            }
            else if (k == HotKeys.B)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                Service.gI().friend(0, -1);
                return true;
            }
            else if (k == HotKeys.X)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                MainMod.ShowMenu();
                return true;
            }
            else if (k == HotKeys.M)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                if (GameCanvas.panel != null && GameCanvas.panel.isShow && GameCanvas.panel.type == 3 && !GameCanvas.panel.isClose && GameCanvas.panel.cmx == 0)
                {
                    GameCanvas.panel.hide();
                }
                else if (global::Char.myCharz().cHP > 0 && global::Char.myCharz().statusMe != 14 && !TileMap.isOfflineMap())
                {
                    Service.gI().openUIZone();
                    if (GameCanvas.panel != null)
                    {
                        GameCanvas.panel.setTypeZone();
                        GameCanvas.panel.show();
                    }
                }
                return true;
            }
            else if (k == HotKeys.C)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                if (global::Char.myCharz().arrItemBag != null)
                {
                    for (int j = 0; j < global::Char.myCharz().arrItemBag.Length; j++)
                    {
                        Item item = global::Char.myCharz().arrItemBag[j];
                        if (item != null && item.template != null && (item.template.id == 194 || item.template.id == 193))
                        {
                            Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                            break;
                        }
                    }
                }
                return true;
            }
            else if (k == HotKeys.F)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                bool isNhapThe = global::Char.myCharz().isNhapThe;
                MainMod.UseItem(454);
                if (isNhapThe)
                {
                    Service.gI().petStatus(3);
                }
                return true;
            }
            else if (k == HotKeys.G)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                if (global::Char.myCharz().charFocus == null)
                {
                    GameScr.info1.addInfo("Vui Lòng Chọn Mục Tiêu!", 0);
                }
                else
                {
                    Service.gI().giaodich(0, global::Char.myCharz().charFocus.charID, -1, -1);
                    GameScr.info1.addInfo("Đã Gửi Lời Mời Giao Dịch Đến: " + global::Char.myCharz().charFocus.cName, 0);
                }
                return true;
            }
            else if (k == HotKeys.J)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                AutoMap.LoadMapLeft();
                return true;
            }
            else if (k == HotKeys.K)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                AutoMap.LoadMapCenter();
                return true;
            }
            else if (k == HotKeys.L)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                AutoMap.LoadMapRight();
                return true;
            }
            else if (k == HotKeys.T)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                MainMod.UseItem(521);
                return true;
            }
            else if (k == HotKeys.V)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                global::Char target = global::Char.myCharz().charFocus;
                if (target != null)
                {
                    TeleportTo(target.cx, target.cy);
                    GameScr.info1.addInfo("Dịch chuyển đến: " + target.cName, 0);
                }
                else if (global::Char.myCharz().mobFocus != null)
                {
                    Mob mob = global::Char.myCharz().mobFocus;
                    TeleportTo(mob.x, mob.y);
                    GameScr.info1.addInfo("Dịch chuyển đến: " + ((mob.getTemplate() != null) ? mob.getTemplate().name : ("Quái " + mob.mobId)), 0);
                }
                else
                {
                    GameScr.info1.addInfo("Chưa chọn mục tiêu!", 0);
                }
                return true;
            }
            else if (k == HotKeys.E)
            {
                GameCanvas.keyAsciiPress = 0;
                GameCanvas.clearKeyPressed();
                MainMod.isAutoRevive = !MainMod.isAutoRevive;
                GameScr.info1.addInfo("Auto Hồi Sinh\n" + (MainMod.isAutoRevive ? "ON" : "OFF"), 0);
                return true;
            }
            return false;
        }
        public static void DrawTextWithBorder(mGraphics g, mFont font, string text, int x, int y, int align, Color textColor)
        {
            if (string.IsNullOrEmpty(text) || font == null)
            {
                return;
            }

            int px = x - 1;
            int py = y - (font.isDynamicFont ? 0 : 2);

            Color oldColor1 = font.color1;
            Color oldColor2 = font.color2;

            try
            {
                // 1. Vẽ viền đen 4 hướng 1px mỏng sắc nét (Crisp 1px Outline)
                font.color1 = Color.black;
                font.color2 = Color.black;
                font._drawString(g, text, px - 1, py, align);
                font._drawString(g, text, px + 1, py, align);
                font._drawString(g, text, px, py - 1, align);
                font._drawString(g, text, px, py + 1, align);

                // 2. Vẽ màu chữ chính ở giữa
                font.color1 = textColor;
                font.color2 = textColor;
                font._drawString(g, text, px, py, align);
            }
            finally
            {
                font.color1 = oldColor1;
                font.color2 = oldColor2;
            }
        }

        public static void DrawTextWithBorder(mGraphics g, mFont fontMain, string text, int x, int y, int align)
        {
            if (fontMain == null)
            {
                return;
            }
            DrawTextWithBorder(g, fontMain, text, x, y, align, fontMain.color1);
        }
        public static void ShowMenu()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Auto Map", MainMod.getInstance(), 1, null));
            myVector.addElement(new Command("Auto Skill", MainMod.getInstance(), 6, null));
            myVector.addElement(new Command("Thông Tin Đệ Tử\n" + (MainMod.ttdt ? "ON " : "OFF"), MainMod.getInstance(), 7, null));
            myVector.addElement(new Command("Thông Tin Phiên Bản", MainMod.getInstance(), 4, null));
            GameCanvas.menu.startAt(myVector, 3);
        }
        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    AutoMap.ShowMenu();
                    return;
                case 2:
                    Application.OpenURL("https://www.facebook.com/TigoDev");
                    return;
                case 3:
                    Application.OpenURL("https://zalo.me/0392266051");
                    return;
                case 4:
                    MyVector myVector = new MyVector();
                    myVector.addElement(new Command("Facebook", MainMod.getInstance(), 2, null));
                    myVector.addElement(new Command("Zalo", MainMod.getInstance(), 3, null));
                    myVector.addElement(new Command("Phiên bản", MainMod.getInstance(), 5, null));
                    GameCanvas.menu.startAt(myVector, 3);
                    return;
                case 5:
                    GameCanvas.startOKDlg(
     "MOD AUTOPRO v0.0.2\n" +
     "Phát triển bởi: Tigo Studio\n\n" +
     "Giá key: 200.000đ / key (sử dụng vĩnh viễn)\n" +
     "Không áp dụng giảm giá hoặc ưu đãi học sinh, sinh viên. " +
     "Không bán key theo tuần, tháng hay ngày.\n\n" +
     "Liên hệ mua key:\n" +
     "- Facebook: Nhân Nguyễn (Tigo Dev)\n" +
     "- Zalo: 0392266051\n\n" +
     "Lưu ý: Tigo Studio chỉ bán key qua các kênh chính thức ở trên. " +
     "Mọi nguồn khác đều là giả mạo, vui lòng cảnh giác để tránh bị lừa đảo.\n\n" +
     "Chúc anh em chơi game vui vẻ!"
 );
                    return;
                case 6:
                    AutoSkill.ShowMenu();
                    return;
                case 7:
                    MainMod.ttdt = !MainMod.ttdt;
                    GameScr.info1.addInfo("Thông Tin Đệ Tử\n" + (MainMod.ttdt ? "ON " : "OFF"), 0);
                    return;
                default:
                    return;
            }
        }
        static MainMod()
        {
            MainMod.runSpeed = 8;
        }

        public static void UpdateThongBao()
        {
            if (GameScr.gI() == null || global::Char.myCharz() == null) return;
            long now = mSystem.currentTimeMillis();
            if (lastTimeThongBao == 0)
            {
                lastTimeThongBao = now;
                try
                {
                    GameScr.gI().chatVip(thongBaoList[thongBaoIndex]);
                    thongBaoIndex = (thongBaoIndex + 1) % thongBaoList.Length;
                }
                catch { }
                return;
            }
            if (now - lastTimeThongBao >= 300000)
            {
                lastTimeThongBao = now;
                try
                {
                    GameScr.gI().chatVip(thongBaoList[thongBaoIndex]);
                    thongBaoIndex = (thongBaoIndex + 1) % thongBaoList.Length;
                }
                catch
                {
                }
            }
        }

        public static void Update()
        {
            try
            {
                UpdateThongBao();
                updateZoneRealTime();
                UpdateAutoZone();

                if (MainMod.isAutoRevive)
                {
                    MainMod.Revive();
                }
                AutoMap.Update();
                AutoSkill.Update();
                if (global::Char.myCharz().cspeed != MainMod.runSpeed)
                {
                    global::Char.myCharz().cspeed = MainMod.runSpeed;
                }
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi MainMod.Update: " + ex);
            }
        }
        public static void Revive()
        {
            if (global::Char.myCharz() == null)
            {
                return;
            }
            if (global::Char.myCharz().luong + global::Char.myCharz().luongKhoa > 0 && global::Char.myCharz().meDead && global::Char.myCharz().cHP <= 0 && GameCanvas.gameTick % 20 == 0)
            {
                Service.gI().wakeUpFromDead();
                global::Char.myCharz().meDead = false;
                global::Char.myCharz().statusMe = 1;
                global::Char.myCharz().cHP = global::Char.myCharz().cHPFull;
                global::Char.myCharz().cMP = global::Char.myCharz().cMPFull;
                global::Char @char = global::Char.myCharz();
                global::Char char2 = global::Char.myCharz();
                global::Char.myCharz().cp3 = 0;
                char2.cp2 = 0;
                @char.cp1 = 0;
                ServerEffect.addServerEffect(109, global::Char.myCharz(), 2);
                GameScr.gI().center = null;
                GameScr.isHaveSelectSkill = true;
            }
        }
        public static void UpdateAutoZone()
        {
            if (autoZoneTarget < 0)
            {
                return;
            }

            if (global::Char.myCharz() == null || global::Char.myCharz().meDead || global::Char.myCharz().statusMe == 14 || TileMap.isOfflineMap())
            {
                autoZoneTarget = -1;
                return;
            }

            if (TileMap.mapID != autoZoneMapId)
            {
                autoZoneTarget = -1;
                return;
            }

            if (TileMap.zoneID == autoZoneTarget)
            {
                GameScr.info1.addInfo("Đã vào khu " + autoZoneTarget + "!", 0);
                autoZoneTarget = -1;
                return;
            }

            long now = mSystem.currentTimeMillis();
            if (now < nextAutoZoneCooldownUntil)
            {
                return;
            }

            if (now - lastAutoZoneTime >= 1500)
            {
                lastAutoZoneTime = now;
                try
                {
                    Service.gI().requestChangeZone(autoZoneTarget, -1);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void updateZoneRealTime()
        {
            // Chỉ cập nhật khi người chơi ĐANG MỞ BẢNG CHỌN KHU (type == 3)
            if (GameCanvas.panel != null && GameCanvas.panel.isShow && GameCanvas.panel.type == 3 && !GameCanvas.panel.isClose && GameCanvas.panel.cmx == 0)
            {
                if (global::Char.myCharz() == null || global::Char.myCharz().meDead || global::Char.myCharz().statusMe == 14 || TileMap.isOfflineMap())
                {
                    return;
                }

                // Cập nhật độ dài danh sách nếu số lượng khu thay đổi mà không làm reset vị trí cuộn hay vị trí chọn
                if (GameScr.gI() != null && GameScr.gI().zones != null && GameCanvas.panel.currentListLength != GameScr.gI().zones.Length)
                {
                    GameCanvas.panel.currentListLength = GameScr.gI().zones.Length;
                    GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * 24 - GameCanvas.panel.hScroll;
                    if (GameCanvas.panel.cmyLim < 0)
                    {
                        GameCanvas.panel.cmyLim = 0;
                    }
                }

                long now = mSystem.currentTimeMillis();
                if (autoZoneTarget >= 0 && now - lastAutoZoneTime < 500)
                {
                    return;
                }
                if (now - lastTimeUpdateZone >= 1500)
                {
                    lastTimeUpdateZone = now;
                    Service.gI().openUIZone();
                }
            }
        }
        public static void UseItem(int templateId)
        {
            for (int i = 0; i < global::Char.myCharz().arrItemBag.Length; i++)
            {
                Item item = global::Char.myCharz().arrItemBag[i];
                if (item != null && (int)item.template.id == templateId)
                {
                    Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                    return;
                }
            }
        }
        public static void TeleportTo(int x, int y)
        {
            if (global::Char.myCharz() == null) return;
            if (TileMap.pxw > 48) x = Mathf.Clamp(x, 24, TileMap.pxw - 24);
            if (TileMap.pxh > 48) y = Mathf.Clamp(y, 24, TileMap.pxh - 24);
            global::Char.myCharz().cx = x;
            global::Char.myCharz().cy = y;
            Service.gI().charMove();
            global::Char.myCharz().cx = x;
            global::Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            global::Char.myCharz().cx = x;
            global::Char.myCharz().cy = y;
            Service.gI().charMove();
        }

        public static int runSpeed;
        public static MainMod _Instance;
        public static bool isAutoRevive;
        private static long lastTimeThongBao = 0;
        private static int thongBaoIndex = 0;
        private static readonly string[] thongBaoList = new string[]
        {
            "Bạn đang sử dụng bản mod AUTOPRO Code By Tigo!",
            "Chúc AE chơi game vui vẻ"
        };
        public static long lastTimeUpdateZone;

        public static int autoZoneTarget = -1;
        public static int autoZoneMapId = -1;
        public static long lastAutoZoneTime = 0;
        public static long nextAutoZoneCooldownUntil = 0;
        public static bool ttdt = false;

        // ─────────────────────────────────────────────
        //  Cấu hình FPS 480 & Google DNS (8.8.8.8 / 8.8.4.4)
        // ─────────────────────────────────────────────
        public const int TargetFPS = 480;

        public static readonly string[] GoogleDnsServers = new string[]
        {
            "8.8.8.8",
            "8.8.4.4"
        };

        private static readonly Dictionary<string, string> dnsCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static readonly object dnsLock = new object();

        public static string ResolveHostWithGoogleDns(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return host;
            }
            host = host.Trim();

            IPAddress parsedIp;
            if (IPAddress.TryParse(host, out parsedIp))
            {
                return host;
            }

            lock (dnsLock)
            {
                string cachedIp;
                if (dnsCache.TryGetValue(host, out cachedIp) && !string.IsNullOrEmpty(cachedIp))
                {
                    return cachedIp;
                }
            }

            for (int i = 0; i < GoogleDnsServers.Length; i++)
            {
                string resolved = QueryGoogleDnsARecord(host, GoogleDnsServers[i], 1500);
                if (!string.IsNullOrEmpty(resolved))
                {
                    lock (dnsLock)
                    {
                        dnsCache[host] = resolved;
                    }
                    return resolved;
                }
            }

            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(host);
                if (addresses != null)
                {
                    for (int i = 0; i < addresses.Length; i++)
                    {
                        if (addresses[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            string ip = addresses[i].ToString();
                            lock (dnsLock)
                            {
                                dnsCache[host] = ip;
                            }
                            return ip;
                        }
                    }
                }
            }
            catch
            {
            }

            return host;
        }

        private static string QueryGoogleDnsARecord(string domain, string dnsServerIp, int timeoutMs)
        {
            try
            {
                using (UdpClient udp = new UdpClient())
                {
                    udp.Client.SendTimeout = timeoutMs;
                    udp.Client.ReceiveTimeout = timeoutMs;
                    IPEndPoint dnsEndpoint = new IPEndPoint(IPAddress.Parse(dnsServerIp), 53);

                    byte[] query = BuildDnsQueryPacket(domain);
                    udp.Send(query, query.Length, dnsEndpoint);

                    IPEndPoint remoteEndpoint = null;
                    byte[] response = udp.Receive(ref remoteEndpoint);
                    return ParseDnsARecordResponse(response, query.Length);
                }
            }
            catch
            {
                return null;
            }
        }

        private static byte[] BuildDnsQueryPacket(string domain)
        {
            string[] labels = domain.Split('.');
            List<byte> packet = new List<byte>(64);
            packet.Add(0x13);
            packet.Add(0x37);
            packet.Add(0x01);
            packet.Add(0x00);
            packet.Add(0x00);
            packet.Add(0x01);
            packet.Add(0x00);
            packet.Add(0x00);
            packet.Add(0x00);
            packet.Add(0x00);
            packet.Add(0x00);
            packet.Add(0x00);

            for (int i = 0; i < labels.Length; i++)
            {
                byte[] labelBytes = Encoding.ASCII.GetBytes(labels[i]);
                packet.Add((byte)labelBytes.Length);
                packet.AddRange(labelBytes);
            }
            packet.Add(0x00);
            packet.Add(0x00);
            packet.Add(0x01);
            packet.Add(0x00);
            packet.Add(0x01);
            return packet.ToArray();
        }

        private static string ParseDnsARecordResponse(byte[] response, int queryLength)
        {
            if (response == null || response.Length < queryLength + 12)
            {
                return null;
            }
            int anCount = (response[6] << 8) | response[7];
            if (anCount <= 0)
            {
                return null;
            }

            int offset = queryLength;
            for (int i = 0; i < anCount && offset < response.Length; i++)
            {
                if ((response[offset] & 0xC0) == 0xC0)
                {
                    offset += 2;
                }
                else
                {
                    while (offset < response.Length && response[offset] != 0)
                    {
                        if ((response[offset] & 0xC0) == 0xC0)
                        {
                            offset++;
                            break;
                        }
                        offset += response[offset] + 1;
                    }
                    offset++;
                }

                if (offset + 10 > response.Length)
                {
                    break;
                }

                int rType = (response[offset] << 8) | response[offset + 1];
                int rdLength = (response[offset + 8] << 8) | response[offset + 9];
                offset += 10;

                if (offset + rdLength > response.Length)
                {
                    break;
                }

                if (rType == 1 && rdLength == 4)
                {
                    return response[offset] + "." + response[offset + 1] + "." + response[offset + 2] + "." + response[offset + 3];
                }

                offset += rdLength;
            }
            return null;
        }
    }
}
