using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mod
{
    // Token: 0x020000CF RID: 207
    public class AutoSkill : IActionListener, IChatable
    {
        // Token: 0x06000AC5 RID: 2757 RVA: 0x00006ED1 File Offset: 0x000050D1
        public static AutoSkill getInstance()
        {
            if (AutoSkill._Instance == null)
            {
                AutoSkill._Instance = new AutoSkill();
            }
            return AutoSkill._Instance;
        }
        public static bool checkSkill(sbyte idSkill)
        {
            for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
            {
                if (Char.myCharz().vSkill.elementAt(i) is Skill skill && skill.template.id == idSkill)
                {
                    return true;
                }
            }
            return false;
        }
        // Token: 0x06000AC6 RID: 2758 RVA: 0x000A6300 File Offset: 0x000A4500
        public static void Update()
        {
            if (AutoSkill.isAutoSendAttack)
            {
                AutoSkill.AutoSendAttack();
            }
            if (AutoSkill.isTrainPet)
            {
                AutoSkill.AutoSkillForPet();
            }
            if (!global::Char.myCharz().meDead)
            {
                for (int i = 0; i < GameScr.keySkill.Length; i++)
                {
                    if (AutoSkill.isAutoUseSkills[i])
                    {
                        AutoSkill.AutoUseSkill(i);
                    }
                }
            }
            if (AutoSkill.isLoadKeySkill && GameCanvas.gameTick % 20 == 0)
            {
                AutoSkill.isLoadKeySkill = false;
                AutoSkill.LoadKeySkills();
            }
            if (AutoSkill.isAutoChangeFocus)
            {
                AutoSkill.AutoChangeFocus();
            }
        }

        // Token: 0x06000AC7 RID: 2759 RVA: 0x000A637C File Offset: 0x000A457C
        public void onChatFromMe(string text, string to)
        {
            if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
            {
                if (ChatTextField.gI().strChat.Equals(AutoSkill.inputDelaykng[0]))
                {
                    try
                    {
                        long num = long.Parse(ChatTextField.gI().tfChat.getText());
                        smkng = num;
                        kng = true;
                        GameScr.info1.addInfo(string.Concat(new string[]
                        {
                            "Dừng Pem Khi Đệ Đạt: ",
                            NinjaUtil.getMoneys(num),
                            " Tiềm Năng"
                        }), 0);
                    }
                    catch
                    {
                        GameScr.info1.addInfo("Vui Lòng Nhập Lại Tiềm Năng!", 0);
                    }
                    AutoSkill.ResetChatTextField();
                    return;
                }
                if (ChatTextField.gI().strChat.Equals(AutoSkill.inputDelay[0]))
                {
                    try
                    {
                        long num2 = long.Parse(ChatTextField.gI().tfChat.getText());
                        AutoSkill.timeAutoSkills[AutoSkill.indexSkillAuto] = num2;
                        AutoSkill.isAutoUseSkills[AutoSkill.indexSkillAuto] = true;
                        GameScr.info1.addInfo(string.Concat(new string[]
                        {
                            "Auto ",
                            GameScr.keySkill[AutoSkill.indexSkillAuto].template.name,
                            ": ",
                            NinjaUtil.getMoneys(num2),
                            " mili giây"
                        }), 0);
                    }
                    catch
                    {
                        GameScr.info1.addInfo("Vui Lòng Nhập Lại Delay!", 0);
                    }
                    AutoSkill.ResetChatTextField();
                    return;
                }
            }
            else
            {
                ChatTextField.gI().isShow = false;
            }
        }

        // Token: 0x06000AC8 RID: 2760 RVA: 0x00004A95 File Offset: 0x00002C95
        public void onCancelChat()
        {
        }

        // Token: 0x06000AC9 RID: 2761 RVA: 0x000A6530 File Offset: 0x000A4730
        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    AutoSkill.isAutoSendAttack = !AutoSkill.isAutoSendAttack;
                    GameScr.info1.addInfo("Tự Đánh\n" + (AutoSkill.isAutoSendAttack ? "ON" : "OFF"), 0);
                    if (AutoSkill.isAutoSendAttack)
                    {
                        AutoSkill.isAutoChangeFocus = false;
                        return;
                    }
                    break;
                case 2:
                    AutoSkill.isTrainPet = !AutoSkill.isTrainPet;
                    GameScr.info1.addInfo("Đánh Khi Đệ Cần\n" + (AutoSkill.isTrainPet ? "ON" : "OFF"), 0);
                    return;
                case 3:
                    {
                        MyVector myVector = new MyVector();
                        for (int i = 0; i < GameScr.keySkill.Length; i++)
                        {
                            myVector.addElement(new Command(((GameScr.keySkill[i] != null) ? GameScr.keySkill[i].template.name : "null") + "\n[" + (i + 1).ToString() + "]\n", AutoSkill.getInstance(), 10, i));
                        }
                        GameCanvas.menu.startAt(myVector, 3);
                        return;
                    }
                case 4:
                    AutoSkill.isAutoShield = !AutoSkill.isAutoShield;
                    GameScr.info1.addInfo("Auto Khiên Pro\n" + (AutoSkill.isAutoShield ? "ON" : "OFF"), 0);
                    return;
                case 5:
                    GameScr.info1.addInfo("Không Hỗ Trợ Lưu Cài Đặt Ở Mục Này!", 0);
                    return;
                case 6:
                    AutoSkill.isAutoChangeFocus = !AutoSkill.isAutoChangeFocus;
                    GameScr.info1.addInfo("Đánh Chuyển Mục Tiêu\n" + (AutoSkill.isAutoChangeFocus ? "ON" : "OFF"), 0);
                    if (AutoSkill.isAutoChangeFocus)
                    {
                        AutoSkill.isAutoSendAttack = false;
                        return;
                    }
                    break;
                case 7:
                    AutoSkill.listTargetAutoChangeFocus.Clear();
                    GameScr.info1.addInfo("Đã Xóa Danh Sách Đánh Chuyển Mục Tiêu!", 0);
                    return;
                case 8:
                    if (global::Char.myCharz().charFocus != null)
                    {
                        AutoSkill.listTargetAutoChangeFocus.Remove(global::Char.myCharz().charFocus);
                        GameScr.info1.addInfo(string.Concat(new string[]
                        {
                        "Đã Xóa ",
                        global::Char.myCharz().charFocus.cName,
                        " [",
                        global::Char.myCharz().charFocus.charID.ToString(),
                        "]"
                        }), 0);
                        return;
                    }
                    break;
                case 9:
                    if (global::Char.myCharz().charFocus != null)
                    {
                        AutoSkill.listTargetAutoChangeFocus.Add(global::Char.myCharz().charFocus);
                        GameScr.info1.addInfo(string.Concat(new string[]
                        {
                        "Đã Thêm ",
                        global::Char.myCharz().charFocus.cName,
                        " [",
                        global::Char.myCharz().charFocus.charID.ToString(),
                        "]"
                        }), 0);
                        return;
                    }
                    break;
                case 10:
                    AutoSkill.ShowMenuAutoSkill((int)p);
                    return;
                case 11:
                    {
                        int num = (int)p;
                        AutoSkill.isAutoUseSkills[num] = !AutoSkill.isAutoUseSkills[num];
                        if (AutoSkill.isAutoUseSkills[num])
                        {
                            AutoSkill.timeAutoSkills[num] = -1L;
                        }
                        GameScr.info1.addInfo("Auto " + GameScr.keySkill[num].template.name + (AutoSkill.isAutoUseSkills[num] ? (": " + NinjaUtil.getMoneys(AutoSkill.timeAutoSkills[num]) + " mili giây") : "\nOFF"), 0);
                        return;
                    }
                case 12:
                    ChatTextField.gI().strChat = AutoSkill.inputDelay[0];
                    ChatTextField.gI().tfChat.name = AutoSkill.inputDelay[1];
                    ChatTextField.gI().startChat2(AutoSkill.getInstance(), string.Empty);
                    AutoSkill.indexSkillAuto = (int)p;
                    return;
                case 13:
                    {
                        int num2 = (int)p;
                        GameScr.keySkill[num2].coolDown = 0;
                        GameScr.keySkill[num2].manaUse = 0;
                        GameScr.info1.addInfo("Đóng Băng " + GameScr.keySkill[num2].template.name, 0);
                        return;
                    }
                case 14:
                    ChatTextField.gI().strChat = AutoSkill.inputDelaykng[0];
                    ChatTextField.gI().tfChat.name = AutoSkill.inputDelaykng[1];
                    ChatTextField.gI().startChat2(AutoSkill.getInstance(), string.Empty);
                    return;
                default:
                    return;
            }
        }
        public static void selectSkill()
        {
            int[] source = new int[4] { 0, 2, 4, 17 };
            Skill selectedSkill = null;
            Skill[] keySkill = GameScr.keySkill;
            foreach (Skill s in keySkill)
            {
                if (s != null && source.Contains(s.template.id))
                {
                    if (s.template.id == 17)
                    {
                        selectedSkill = s;
                        break;
                    }
                    if (selectedSkill == null)
                    {
                        selectedSkill = s;
                    }
                }
            }
            if (selectedSkill != null && selectedSkill != Char.myCharz().myskill)
            {
                GameScr.gI().doSelectSkill(selectedSkill, isShortcut: true);
            }
        }

        // Token: 0x06000ACA RID: 2762 RVA: 0x000A6968 File Offset: 0x000A4B68
        public static void ShowMenu()
        {
            AutoSkill.LoadData();
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Tự Đánh\n" + (AutoSkill.isAutoSendAttack ? "ON" : "OFF"), AutoSkill.getInstance(), 1, null));
            myVector.addElement(new Command("Đánh Khi Đệ Cần\n" + (AutoSkill.isTrainPet ? "ON" : "OFF"), AutoSkill.getInstance(), 2, null));
            myVector.addElement(new Command(GameScr.keySkill.Length.ToString() + " Ô Kỹ Năng", AutoSkill.getInstance(), 3, null));
            myVector.addElement(new Command("Lưu Cài Đặt\n" + (AutoSkill.isSaveData ? "ON" : "OFF"), AutoSkill.getInstance(), 5, null));
            myVector.addElement(new Command("Đánh Chuyển Mục Tiêu\n" + (AutoSkill.isAutoChangeFocus ? "ON" : "OFF"), AutoSkill.getInstance(), 6, null));
            if (AutoSkill.listTargetAutoChangeFocus.Count > 0)
            {
                myVector.addElement(new Command("Clear Danh Sách Chuyển Mục Tiêu", AutoSkill.getInstance(), 7, null));
            }
            if (global::Char.myCharz().charFocus != null)
            {
                if (AutoSkill.listTargetAutoChangeFocus.Contains(global::Char.myCharz().charFocus))
                {
                    myVector.addElement(new Command("Xóa Khỏi Danh Sách Chuyển Mục Tiêu", AutoSkill.getInstance(), 8, null));
                }
                else
                {
                    myVector.addElement(new Command("Thêm Vào Danh Sách Chuyển Mục Tiêu", AutoSkill.getInstance(), 9, null));
                }
            }
            GameCanvas.menu.startAt(myVector, 3);
        }

        // Token: 0x06000ACB RID: 2763 RVA: 0x000A6AEC File Offset: 0x000A4CEC
        private static void ShowMenuAutoSkill(int skillIndex)
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Auto Sử Dụng\n" + (AutoSkill.isAutoUseSkills[skillIndex] ? ("[" + NinjaUtil.getMoneys(AutoSkill.timeAutoSkills[skillIndex]) + " mili giây]") : "OFF"), AutoSkill.getInstance(), 11, skillIndex));
            myVector.addElement(new Command("Nhập Delay\n[mili giây]", AutoSkill.getInstance(), 12, skillIndex));
            myVector.addElement(new Command("Đóng Băng\n" + GameScr.keySkill[skillIndex].template.name, AutoSkill.getInstance(), 13, skillIndex));
            GameCanvas.menu.startAt(myVector, 3);
        }

        // Token: 0x06000ACC RID: 2764 RVA: 0x00006EE9 File Offset: 0x000050E9
        private static void ResetChatTextField()
        {
            ChatTextField.gI().strChat = "Chat";
            ChatTextField.gI().tfChat.name = "chat";
            ChatTextField.gI().isShow = false;
            Res.outz("CHAT");
        }

        // Token: 0x06000ACD RID: 2765 RVA: 0x00004A95 File Offset: 0x00002C95
        private static void LoadData()
        {
        }

        // Token: 0x06000ACE RID: 2766 RVA: 0x00004A95 File Offset: 0x00002C95
        private static void smethod_6()
        {
        }

        // Token: 0x06000ACF RID: 2767 RVA: 0x000A6BA8 File Offset: 0x000A4DA8
        private static void LoadKeySkills()
        {
            for (int i = 0; i < global::Char.myCharz().nClass.skillTemplates.Length; i++)
            {
                SkillTemplate skillTemplate = global::Char.myCharz().nClass.skillTemplates[i];
                Skill skill = global::Char.myCharz().getSkill(skillTemplate);
                if (skill != null)
                {
                    GameScr.keySkill[i] = skill;
                }
                GameScr.gI().saveKeySkillToRMS();
            }
        }

        // Token: 0x06000AD0 RID: 2768 RVA: 0x000A6C04 File Offset: 0x000A4E04
        public static void AutoSendAttack()
        {
            if (!global::Char.myCharz().meDead && global::Char.myCharz().cHP > 0 && global::Char.myCharz().statusMe != 14 && global::Char.myCharz().statusMe != 5 && global::Char.myCharz().myskill.template.type != 3 && global::Char.myCharz().myskill.template.id != 10 && global::Char.myCharz().myskill.template.id != 11 && (!global::Char.myCharz().myskill.paintCanNotUseSkill || GameCanvas.panel.isShow))
            {
                int mySkillIndex = AutoSkill.GetMySkillIndex();
                if (mSystem.currentTimeMillis() - AutoSkill.lastTimeSendAttack[mySkillIndex] > AutoSkill.GetCoolDown(global::Char.myCharz().myskill))
                {
                    if (GameScr.gI().isMeCanAttackMob(global::Char.myCharz().mobFocus))
                    {
                        global::Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                        AutoSkill.SendAttackToMobFocus();
                        AutoSkill.lastTimeSendAttack[mySkillIndex] = mSystem.currentTimeMillis();
                        return;
                    }
                    if (global::Char.myCharz().charFocus != null && AutoSkill.isMeCanAttackChar(global::Char.myCharz().charFocus) && (double)global::Math.abs(global::Char.myCharz().charFocus.cx - global::Char.myCharz().cx) < (double)global::Char.myCharz().myskill.dx * 1.7)
                    {
                        global::Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                        AutoSkill.SendAttackToCharFocus();
                        AutoSkill.lastTimeSendAttack[mySkillIndex] = mSystem.currentTimeMillis();
                    }
                }
            }
        }

        // Token: 0x06000AD1 RID: 2769 RVA: 0x000A6DA4 File Offset: 0x000A4FA4
        private static void AutoSkillForPet()
        {
            if (AutoSkill.isPetAskedForUseSkill && GameScr.vMob.size() != 0 && global::Char.myCharz().myskill.template.type != 3)
            {
                Mob mobFocus = (Mob)GameScr.vMob.elementAt(0);
                int num = 0;
                for (int i = 0; i < GameScr.vMob.size(); i++)
                {
                    Mob mob = (Mob)GameScr.vMob.elementAt(i);
                    int num2 = global::Math.abs(global::Char.myCharz().cx - mob.x);
                    int num3 = global::Math.abs(global::Char.myCharz().cy - mob.y);
                    int num4 = num2 * num2 + num3 * num3;
                    if (num < num4)
                    {
                        num = num4;
                        mobFocus = mob;
                    }
                }
                global::Char.myCharz().mobFocus = mobFocus;
                int mySkillIndex = AutoSkill.GetMySkillIndex();
                if (mSystem.currentTimeMillis() - AutoSkill.lastTimeSendAttack[mySkillIndex] > (long)(global::Char.myCharz().myskill.coolDown + 100) && GameScr.gI().isMeCanAttackMob(global::Char.myCharz().mobFocus))
                {
                    AutoSkill.SendAttackToMobFocus();
                    AutoSkill.lastTimeSendAttack[mySkillIndex] = mSystem.currentTimeMillis();
                    AutoSkill.isPetAskedForUseSkill = false;
                }
            }
        }

        // Token: 0x06000AD2 RID: 2770 RVA: 0x000A6EC4 File Offset: 0x000A50C4
        private static void AutoUseSkill(int skillIndex)
        {
            if (TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23)
            {
                if (skillIndex >= GameScr.keySkill.Length)
                {
                    skillIndex = GameScr.keySkill.Length - 1;
                }
                if (skillIndex < 0)
                {
                    skillIndex = 0;
                }
                if (GameScr.keySkill[skillIndex] != null && !GameScr.keySkill[skillIndex].paintCanNotUseSkill)
                {
                    if (GameScr.keySkill[skillIndex].coolDown == 0)
                    {
                        AutoSkill.timeAutoSkills[skillIndex] = 500L;
                        return;
                    }
                    if (AutoSkill.isMeHasEnoughMP(GameScr.keySkill[skillIndex]) && !GameScr.gI().isCharging() && mSystem.currentTimeMillis() - AutoSkill.lastTimeAutoUseSkill > 150L)
                    {
                        if (AutoSkill.timeAutoSkills[skillIndex] == -1L && GameCanvas.gameTick % 20 == 0)
                        {
                            AutoSkill.lastTimeUseSkill[skillIndex] = mSystem.currentTimeMillis();
                            AutoSkill.lastTimeAutoUseSkill = mSystem.currentTimeMillis();
                            GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], true);
                        }
                        if (mSystem.currentTimeMillis() - AutoSkill.lastTimeUseSkill[skillIndex] > AutoSkill.timeAutoSkills[skillIndex])
                        {
                            AutoSkill.lastTimeUseSkill[skillIndex] = mSystem.currentTimeMillis();
                            AutoSkill.lastTimeAutoUseSkill = mSystem.currentTimeMillis();
                            GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], true);
                        }
                    }
                }
            }
        }

        // Token: 0x06000AD3 RID: 2771 RVA: 0x000A6FF8 File Offset: 0x000A51F8
        public static bool isMeCanAttackChar(global::Char ch)
        {
            bool result;
            if (TileMap.mapID == 113)
            {
                result = (ch != null && global::Char.myCharz().myskill != null && (ch.cTypePk == 5 || ch.cTypePk == 3));
            }
            else
            {
                result = (ch != null && global::Char.myCharz().myskill != null && ((ch.statusMe != 14 && ch.statusMe != 5 && global::Char.myCharz().myskill.template.type != 2 && ((global::Char.myCharz().cFlag == 8 && ch.cFlag != 0) || (global::Char.myCharz().cFlag != 0 && ch.cFlag == 8) || (global::Char.myCharz().cFlag != ch.cFlag && global::Char.myCharz().cFlag != 0 && ch.cFlag != 0) || (ch.cTypePk == 3 && global::Char.myCharz().cTypePk == 3) || global::Char.myCharz().cTypePk == 5 || ch.cTypePk == 5 || (global::Char.myCharz().cTypePk == 1 && ch.cTypePk == 1) || (global::Char.myCharz().cTypePk == 4 && ch.cTypePk == 4))) || (global::Char.myCharz().myskill.template.type == 2 && ch.cTypePk != 5)));
            }
            return result;
        }

        // Token: 0x06000AD4 RID: 2772 RVA: 0x000A7164 File Offset: 0x000A5364
        private static bool isMeHasEnoughMP(Skill skillToUse)
        {
            bool result;
            if (skillToUse.template.manaUseType == 2)
            {
                result = true;
            }
            else if (skillToUse.template.manaUseType != 1)
            {
                result = (global::Char.myCharz().cMP >= skillToUse.manaUse);
            }
            else
            {
                result = (global::Char.myCharz().cMP >= skillToUse.manaUse * global::Char.myCharz().cMPFull / 100);
            }
            return result;
        }

        // Token: 0x06000AD5 RID: 2773 RVA: 0x000A71D0 File Offset: 0x000A53D0
        private static void SendAttackToCharFocus()
        {
            try
            {
                MyVector myVector = new MyVector();
                myVector.addElement(global::Char.myCharz().charFocus);
                Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
            }
            catch
            {
            }
        }

        // Token: 0x06000AD6 RID: 2774 RVA: 0x000A721C File Offset: 0x000A541C
        private static void SendAttackToMobFocus()
        {
            try
            {
                MyVector myVector = new MyVector();
                myVector.addElement(global::Char.myCharz().mobFocus);
                Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
            }
            catch
            {
            }
        }

        // Token: 0x06000AD7 RID: 2775 RVA: 0x000A7268 File Offset: 0x000A5468
        private static long GetCoolDown(Skill skill)
        {
            long result;
            if (skill.template.id != 20 && skill.template.id != 22 && skill.template.id != 7 && skill.template.id != 18 && skill.template.id != 23)
            {
                result = (long)(skill.coolDown + 100);
            }
            else
            {
                result = (long)skill.coolDown + 500L;
            }
            return result;
        }

        // Token: 0x06000AD8 RID: 2776 RVA: 0x000A72DC File Offset: 0x000A54DC
        private static int GetMySkillIndex()
        {
            for (int i = 0; i < GameScr.keySkill.Length; i++)
            {
                if (GameScr.keySkill[i] == global::Char.myCharz().myskill)
                {
                    return i;
                }
            }
            return 0;
        }

        // Token: 0x06000AD9 RID: 2777 RVA: 0x000A7314 File Offset: 0x000A5514
        private static void AutoChangeFocus()
        {
            if (AutoSkill.listTargetAutoChangeFocus.Count == 0)
            {
                GameScr.info1.addInfo("Danh sách chuyển mục tiêu trống!", 0);
                AutoSkill.isAutoChangeFocus = false;
                return;
            }
            if (!global::Char.myCharz().meDead && global::Char.myCharz().statusMe != 14 && global::Char.myCharz().statusMe != 5 && global::Char.myCharz().myskill.template.type != 3 && global::Char.myCharz().myskill.template.id != 10 && global::Char.myCharz().myskill.template.id != 11 && !global::Char.myCharz().myskill.paintCanNotUseSkill)
            {
                AutoSkill.cooldownAutoChangeFocus = AutoSkill.GetCooldownAutoChangeFocus(global::Char.myCharz().myskill);
                if (AutoSkill.targetIndex >= AutoSkill.listTargetAutoChangeFocus.Count)
                {
                    AutoSkill.targetIndex = 0;
                }
                if (mSystem.currentTimeMillis() - AutoSkill.lastTimeChangeFocus >= AutoSkill.cooldownAutoChangeFocus)
                {
                    AutoSkill.lastTimeChangeFocus = mSystem.currentTimeMillis();
                    global::Char.myCharz().charFocus = GameScr.findCharInMap(AutoSkill.listTargetAutoChangeFocus[AutoSkill.targetIndex].charID);
                    AutoSkill.targetIndex++;
                    if (AutoSkill.targetIndex >= AutoSkill.listTargetAutoChangeFocus.Count)
                    {
                        AutoSkill.targetIndex = 0;
                    }
                    if (global::Char.myCharz().charFocus != null && AutoSkill.isMeCanAttackChar(global::Char.myCharz().charFocus) && (double)global::Math.abs(global::Char.myCharz().charFocus.cx - global::Char.myCharz().cx) < (double)global::Char.myCharz().myskill.dx * 1.5)
                    {
                        global::Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                        AutoSkill.SendAttackToCharFocus();
                    }
                }
            }
        }

        // Token: 0x06000ADA RID: 2778 RVA: 0x000A74DC File Offset: 0x000A56DC
        private static long GetCooldownAutoChangeFocus(Skill skill)
        {
            long result;
            if (skill.coolDown <= 500)
            {
                result = 1000L;
            }
            else
            {
                result = (long)((double)skill.coolDown * 1.2 + 200.0);
            }
            return result;
        }

        // Token: 0x06000ADB RID: 2779 RVA: 0x00004A95 File Offset: 0x00002C95
        private static void smethod_0()
        {
        }

        // Token: 0x06000ADC RID: 2780 RVA: 0x000A7520 File Offset: 0x000A5720
        static AutoSkill()
        {
            AutoSkill.LoadData();
        }

        // Token: 0x04001392 RID: 5010
        private static AutoSkill _Instance;

        // Token: 0x04001393 RID: 5011
        public static bool isLoadKeySkill = true;

        // Token: 0x04001394 RID: 5012
        public static bool isAutoSendAttack;

        // Token: 0x04001395 RID: 5013
        private static long[] lastTimeSendAttack = new long[10];

        // Token: 0x04001396 RID: 5014
        public static bool isTrainPet;

        // Token: 0x04001397 RID: 5015
        public static bool isPetAskedForUseSkill;

        // Token: 0x04001398 RID: 5016
        public static bool[] isAutoUseSkills = new bool[10];

        // Token: 0x04001399 RID: 5017
        private static long[] lastTimeUseSkill = new long[10];

        // Token: 0x0400139A RID: 5018
        private static long[] timeAutoSkills = new long[10];

        // Token: 0x0400139B RID: 5019
        private static int indexSkillAuto;

        // Token: 0x0400139C RID: 5020
        private static bool isAutoChangeFocus;

        // Token: 0x0400139D RID: 5021
        private static long cooldownAutoChangeFocus;

        // Token: 0x0400139E RID: 5022
        private static long lastTimeChangeFocus;

        // Token: 0x0400139F RID: 5023
        private static List<global::Char> listTargetAutoChangeFocus = new List<global::Char>();

        // Token: 0x040013A0 RID: 5024
        private static int targetIndex;

        // Token: 0x040013A1 RID: 5025
        private static bool isAutoShield;
        public static bool kng;

        // Token: 0x040013A2 RID: 5026
        private static string[] inputDelay = new string[]
        {
            "Nhập delay",
            "mili giây"
        };

        // Token: 0x040013A3 RID: 5027
        private static bool isSaveData;

        // Token: 0x040013A4 RID: 5028
        private static long lastTimeAutoUseSkill;
        public static long smkng;

        // Token: 0x040013A5 RID: 5029
        private static string[] inputDelaykng = new string[]
        {
            "Nhập Tiềm Năng Muốn Đệ Dừng Pem",
            "Sức Mạnh"
        };
    }
}
