using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    public partial class AddGameApiVersionAndLangCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100029' WHERE \"UpdateStats\"='Update24'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100030' WHERE \"UpdateStats\"='Update25'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100031' WHERE \"UpdateStats\"='Update26'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100031' WHERE \"UpdateStats\"='Update26-Prod'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100032' WHERE \"UpdateStats\"='Update27'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100033' WHERE \"UpdateStats\"='Update28'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100034' WHERE \"UpdateStats\"='Update29'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='100035' WHERE \"UpdateStats\"='Update30'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='101031' WHERE \"UpdateStats\"='Update31'");
            migrationBuilder.Sql("UPDATE \"Langtexts\" SET \"UpdateStats\"='101032' WHERE \"UpdateStats\"='Update32'");

            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100029' WHERE \"UpdateStats\"='Update24'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100030' WHERE \"UpdateStats\"='Update25'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100031' WHERE \"UpdateStats\"='Update26'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100031' WHERE \"UpdateStats\"='Update26-Prod'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100032' WHERE \"UpdateStats\"='Update27'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100033' WHERE \"UpdateStats\"='Update28'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100034' WHERE \"UpdateStats\"='Update29'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='100035' WHERE \"UpdateStats\"='Update30'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='101031' WHERE \"UpdateStats\"='Update31'");
            migrationBuilder.Sql("UPDATE \"LangtextReview\" SET \"UpdateStats\"='101032' WHERE \"UpdateStats\"='Update32'");

            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100029' WHERE \"UpdateStats\"='Update24'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100030' WHERE \"UpdateStats\"='Update25'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100031' WHERE \"UpdateStats\"='Update26'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100031' WHERE \"UpdateStats\"='Update26-Prod'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100032' WHERE \"UpdateStats\"='Update27'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100033' WHERE \"UpdateStats\"='Update28'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100034' WHERE \"UpdateStats\"='Update29'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='100035' WHERE \"UpdateStats\"='Update30'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='101031' WHERE \"UpdateStats\"='Update31'");
            migrationBuilder.Sql("UPDATE \"LangtextArchive\" SET \"UpdateStats\"='101032' WHERE \"UpdateStats\"='Update32'");

            migrationBuilder.Sql("ALTER TABLE \"Langtexts\" ALTER COLUMN \"UpdateStats\" TYPE int4 USING \"UpdateStats\"::int4;");
            migrationBuilder.Sql("ALTER TABLE \"LangtextReview\" ALTER COLUMN \"UpdateStats\" TYPE int4 USING \"UpdateStats\"::int4;");
            migrationBuilder.Sql("ALTER TABLE \"LangtextArchive\" ALTER COLUMN \"UpdateStats\" TYPE int4 USING \"UpdateStats\"::int4;");

            migrationBuilder.RenameColumn(
                name: "UpdateStats",
                table: "Langtexts",
                newName: "GameApiVersion");

            migrationBuilder.RenameColumn(
                name: "UpdateStats",
                table: "LangtextReview",
                newName: "GameApiVersion");

            migrationBuilder.RenameColumn(
                name: "UpdateStats",
                table: "LangtextArchive",
                newName: "GameApiVersion");

            migrationBuilder.CreateTable(
                name: "GameVersion",
                columns: table => new
                {
                    GameApiVersion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Version_EN = table.Column<string>(type: "text", nullable: true),
                    Version_ZH = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersion", x => x.GameApiVersion);
                });

            migrationBuilder.CreateTable(
                name: "LangTypeCatalog",
                columns: table => new
                {
                    IdType = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTypeZH = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangTypeCatalog", x => x.IdType);
                });

            migrationBuilder.InsertData(
                table: "GameVersion",
                columns: new[] { "GameApiVersion", "Version_EN", "Version_ZH" },
                values: new object[,]
                {
                    { "100029", "Dragonhold", "龙堡" },
                    { "100030", "Harrowstorm", "苦痛风暴" },
                    { "100031", "Greymoor", "灰沼" },
                    { "100032", "Stonethorn", "石之荆棘" },
                    { "100033", "Markarth", "马卡斯" },
                    { "100034", "Flames of Ambition", "野心之焰" },
                    { "100035", "Blackwood", "黑森林" },
                    { "101031", "Waking Flame", "觉醒之火" },
                    { "101032", "Deadlands", "死地" },
                });

            migrationBuilder.InsertData(
                table: "LangTypeCatalog",
                columns: new[] { "IdType", "IdTypeZH" },
                values: new object[,]
                {
                    {"100","Lua UI"},
                    {"3427285","表情预览"},
                    {"3952276","任务-交谈(长)"},
                    {"4330293","地图名"},
                    {"4922190","tip"},
                    {"5759525","more-ui"},
                    {"6658117","交互(解锁)"},
                    {"7949764","任务目标"},
                    {"8158238","交互(场景物品)"},
                    {"8290981","NPC名字"},
                    {"8379076","more-desc"},
                    {"8637315","8637315"},
                    {"9019316","more-ui"},
                    {"9424005","more-ui"},
                    {"10860933","地标名(大地图)"},
                    {"11547061","item-type"},
                    {"12320021","交互(背包内物品)"},
                    {"12529189","成就名"},
                    {"12738702","12738702"},
                    {"12912341","交互(场景NPC与物品)"},
                    {"13753646","tip"},
                    {"14464837","陷阱"},
                    {"15453358","考古线索"},
                    {"15900245","15900245"},
                    {"17915077","more-ui"},
                    {"18104308","tip"},
                    {"18173141","收藏品名"},
                    {"19398485","互动物品（按E）"},
                    {"19709733","等级解锁奖励（副本名）"},
                    {"20958740","接受任务选项"},
                    {"21337012","书籍信件内容"},
                    {"22296053","title"},
                    {"24991438","tip"},
                    {"26044436","tip"},
                    {"26811173","more-ui"},
                    {"28666901","location-and-object"},
                    {"30721042","more-ui"},
                    {"33425332","邮件寄件人名（NPC）"},
                    {"34157141","more-ui"},
                    {"34717246","交互(重物品)"},
                    {"35111812","tip"},
                    {"37288388","tip"},
                    {"37408565","more-ui"},
                    {"38727365","套装名"},
                    {"39160885","藏宝图名"},
                    {"39248996","tip"},
                    {"39619172","location-and-object"},
                    {"40741187","more-ui"},
                    {"41262789","item-type"},
                    {"41714900","tip"},
                    {"41983653","item-type"},
                    {"42041397","more-ui"},
                    {"43934149","tip"},
                    {"44699029","more-ui"},
                    {"45275092","交互(合上书籍)"},
                    {"45378037","more-ui"},
                    {"45608037","45608037"},
                    {"46427668","tip"},
                    {"49656629","49656629"},
                    {"50040644","more-desc"},
                    {"50143374","tip"},
                    {"51029557","51029557"},
                    {"51109077","more-ui"},
                    {"51188213","书籍信件名"},
                    {"51188660","NPC名（变量代码）"},
                    {"52183620","more-desc"},
                    {"52420949","日志任务名"},
                    {"54595589","装备(神器)名"},
                    {"55049764","任务-交谈"},
                    {"56212707","56212707"},
                    {"56558612","tip"},
                    {"57008677","location-and-object"},
                    {"57010981","more-ui"},
                    {"58548677","tip"},
                    {"59621621","item-type"},
                    {"59991493","放置的家具互动"},
                    {"60008005","tip"},
                    {"60139732","tip"},
                    {"60155541","title"},
                    {"61533042","item-type"},
                    {"62156964","tip"},
                    {"63563637","title"},
                    {"63937076","tip"},
                    {"65447205","more-ui"},
                    {"66737390","任务-与NPC交谈"},
                    {"66848564","任务NPC打招呼"},
                    {"67804083","67804083"},
                    {"68494373","item-type"},
                    {"68561141","more-ui"},
                    {"70307621","交互(场景物品)"},
                    {"70328405","皇冠物品名"},
                    {"70901198","载屏描述"},
                    {"71626837","more-ui"},
                    {"71931413","more-ui"},
                    {"73074773","宝物名(?)"},
                    {"74148292","tip"},
                    {"74865733","交互(场景物品)"},
                    {"75236676","NPC打招呼"},
                    {"75237444","NPC打招呼2"},
                    {"75238212","NPC打招呼3"},
                    {"75240772","NPC打招呼4"},
                    {"75241540","NPC打招呼5"},
                    {"75242308","NPC打招呼6"},
                    {"75244868","greeting"},
                    {"75245636","greeting"},
                    {"75246404","greeting"},
                    {"75248964","greeting"},
                    {"75249732","greeting"},
                    {"75250500","greeting"},
                    {"75253060","greeting"},
                    {"75253828","greeting"},
                    {"75254596","greeting"},
                    {"75257156","greeting"},
                    {"75257924","greeting"},
                    {"75258692","greeting"},
                    {"75261252","greeting"},
                    {"75262020","greeting"},
                    {"75262788","greeting"},
                    {"75265348","greeting"},
                    {"75266116","greeting"},
                    {"75266884","greeting"},
                    {"76200101","location-and-object"},
                    {"76698596","76698596"},
                    {"77659573","location-and-object"},
                    {"78205445","聊天预置语句"},
                    {"79246725","more-ui"},
                    {"81344020","location-and-object"},
                    {"81761156","tip"},
                    {"83548836","83548836"},
                    {"84281828","tip"},
                    {"86601028","tip"},
                    {"87370069","location-and-object"},
                    {"87522148","title"},
                    {"87722757","87722757"},
                    {"90431749","奇怪的NPC名"},
                    {"91126884","tip"},
                    {"93314261","93314261"},
                    {"96069573","npc-name"},
                    {"96678629","决斗场地图代码"},
                    {"96962005","more-ui"},
                    {"98383029","item-type"},
                    {"99155012","greeting"},
                    {"99281989","节日活动名"},
                    {"99527054","more-ui"},
                    {"101034709","more-ui"},
                    {"101286772","tip"},
                    {"102062948","tip"},
                    {"102906708","102906708"},
                    {"103224356","任务-交谈(长)3"},
                    {"104708420","tip"},
                    {"106360516","tip"},
                    {"106474997","more-ui"},
                    {"107182786","107182786"},
                    {"108533454","交互(交互中)"},
                    {"108566804","已完成的地标说明"},
                    {"108643301","more-ui"},
                    {"108901797","108901797"},
                    {"108965317","108965317"},
                    {"109216308","交互(离开交互)"},
                    {"111863941","more-ui"},
                    {"112701171","more-ui"},
                    {"112758405","more-ui"},
                    {"114933028","114933028"},
                    {"115318052","tip"},
                    {"115337253","成就分类二级菜单"},
                    {"115391780","tip"},
                    {"115740052","任务-NPC对白"},
                    {"116521668","quest-end"},
                    {"116704773","阵营名"},
                    {"117426949","117426949"},
                    {"117539474","tip"},
                    {"119308740","119308740"},
                    {"121487972","任务-与NPC交谈2"},
                    {"121548292","tip"},
                    {"121778053","聊天预置语句"},
                    {"121975845","地图名"},
                    {"123229230","tip"},
                    {"124119973","附魔词缀"},
                    {"124318053","more-ui"},
                    {"124362421","皇冠宝箱名"},
                    {"125518133","more-ui"},
                    {"125568292","125568292"},
                    {"127454222","more-desc"},
                    {"129382708","tip"},
                    {"129979412","任务目标(在HUD右边)"},
                    {"130098181","130098181"},
                    {"131421317","more-ui"},
                    {"132143172","技能描述"},
                    {"132595845","tip"},
                    {"135729940","tip"},
                    {"137743365","137743365"},
                    {"139139780","任务物品描述"},
                    {"139475237","收藏品-表情"},
                    {"139528164","tip"},
                    {"139757006","tip"},
                    {"140248372","140248372"},
                    {"141135108","tip"},
                    {"142011652","tip"},
                    {"143348165","title"},
                    {"143563989","more-ui"},
                    {"143794484","143794484"},
                    {"143811061","more-ui"},
                    {"144228340","走私任务?(不确定)"},
                    {"144446238","tip"},
                    {"145410824","tip"},
                    {"145684164","tip"},
                    {"146361138","大地图地标名"},
                    {"148042590","148042590"},
                    {"148355781","more-ui"},
                    {"148453652","tip"},
                    {"149328292","NPC对话"},
                    {"149979604","greeting"},
                    {"149983700","greeting"},
                    {"149987796","greeting"},
                    {"149991892","greeting"},
                    {"149995988","greeting"},
                    {"150000084","greeting"},
                    {"150004180","greeting"},
                    {"150008276","greeting"},
                    {"150045140","greeting"},
                    {"150049236","greeting"},
                    {"150053332","greeting"},
                    {"150057428","greeting"},
                    {"150061524","greeting"},
                    {"150065620","greeting"},
                    {"150069716","greeting"},
                    {"150073812","greeting"},
                    {"150525940","greeting"},
                    {"150962644","greeting"},
                    {"150966740","greeting"},
                    {"150970836","greeting"},
                    {"150974932","greeting"},
                    {"150979028","greeting"},
                    {"150983124","greeting"},
                    {"150987220","greeting"},
                    {"150991316","greeting"},
                    {"151600453","more-ui"},
                    {"151638485","151638485"},
                    {"151931684","more-desc"},
                    {"152988005","国家与地区名"},
                    {"153008933","153008933"},
                    {"153349653","tip"},
                    {"155022052","tip"},
                    {"156152165","more-ui"},
                    {"156570558","星座描述"},
                    {"156664686","tip"},
                    {"157886597","战场地标"},
                    {"158979221","more-ui"},
                    {"160227428","tip"},
                    {"160647118","160647118"},
                    {"162144901","more-ui"},
                    {"162658389","载屏地图名"},
                    {"162946485","location-and-object"},
                    {"164009093","地点（按E互动）"},
                    {"164317956","more-desc"},
                    {"164328533","tip"},
                    {"164387044","tip"},
                    {"165399380","NPC谈话"},
                    {"167361812","167361812"},
                    {"167432014","167432014"},
                    {"168351172","tip"},
                    {"168415844","quest-end"},
                    {"168675493","tip"},
                    {"169578494","房屋载屏描述"},
                    {"169602884","greeting"},
                    {"171157587","more-desc"},
                    {"172030117","成就分项目标"},
                    {"172689156","tip"},
                    {"173340693","more-ui"},
                    {"180809749","more-ui"},
                    {"184479092","184479092"},
                    {"185724645","tip"},
                    {"186232436","title"},
                    {"187173764","任务-NPC对白2"},
                    {"188095652","交互(未知)"},
                    {"188155806","成就总体目标"},
                    {"188513717","more-ui"},
                    {"191189508","more-desc"},
                    {"191379205","more-ui"},
                    {"191744852","tip"},
                    {"191999749","npc-name"},
                    {"193511764","tip"},
                    {"193678788","tip"},
                    {"196014052","more-ui"},
                    {"198758357","技能名"},
                    {"199723588","tip"},
                    {"200374766","载屏描述2"},
                    {"200521140","more-ui"},
                    {"200697509","more-ui"},
                    {"200879108","任务-交谈(长)2"},
                    {"202153303","tip"},
                    {"203274254","more-ui"},
                    {"204530069","more-ui"},
                    {"204987124","任务-与NPC交谈的选择"},
                    {"205344756","任务目标描述"},
                    {"206046340","more-desc"},
                    {"207398837","npc-name"},
                    {"207758933","more-ui"},
                    {"208337109","染色名"},
                    {"210579221","210579221"},
                    {"211640654","收藏品描述"},
                    {"211899940","location-and-object"},
                    {"212113054","tip"},
                    {"214390738","item-type"},
                    {"215700677","title"},
                    {"216055893","more-ui"},
                    {"216271813","more-ui"},
                    {"217086453","more-ui"},
                    {"217370677","item-type"},
                    {"219317028","letter"},
                    {"219429541","more-ui"},
                    {"219689294","交互(场景魔法物品？)"},
                    {"219691006","219691006"},
                    {"219936053","采集调查地点（按E互动）"},
                    {"220262196","tip"},
                    {"221172404","tip"},
                    {"221887989","title"},
                    {"224768149","more-ui"},
                    {"224875171","more-desc"},
                    {"224972965","more-ui"},
                    {"225762485","225762485"},
                    {"226966585","tip"},
                    {"227804446","227804446"},
                    {"228103012","任务-与NPC交谈的选择2"},
                    {"228378404","装备物品描述"},
                    {"229689221","229689221"},
                    {"230486948","tip"},
                    {"232026500","接受任务选项2"},
                    {"232566869","more-ui"},
                    {"234260606","234260606"},
                    {"234743124","未知(任务相关?)"},
                    {"235463860","阵营描述"},
                    {"235850260","tip"},
                    {"236931909","more-ui"},
                    {"237304340","tip"},
                    {"238195765","238195765"},
                    {"239667646","239667646"},
                    {"239939829","开发用文本(无需翻译)"},
                    {"241484741","more-ui"},
                    {"242643895","more-ui"},
                    {"242841733","装备物品名"},
                    {"243094948","tip"},
                    {"244251267","244251267"},
                    {"246790420","tip"},
                    {"247934532","location-and-object"},
                    {"248074243","248074243"},
                    {"249464990","249464990"},
                    {"249633428","tip"},
                    {"249673710","皇冠宝箱描述"},
                    {"249936564","接受任务选项3"},
                    {"251542164","tip"},
                    {"252100948","tip"},
                    {"253017305","tip"},
                    {"254784612","tip"},
                    {"256430276","任务目标2"},
                    {"256705124","tip"},
                    {"257983733","more-ui"},
                    {"259128606","more-ui"},
                    {"259945604","greeting"},
                    {"259956452","tip"},
                    {"260523861","location-and-object"},
                    {"263004526","交互(交互中)2"},
                    {"263796174","皇冠物品描述"},
                    {"264248485","264248485"},
                    {"264355726","tip"},
                    {"265851556","日志任务描述"},
                    {"266730334","tip"},
                    {"266968996","266968996"},
                    {"267200725","location-and-object"},
                    {"267697733","任务物品名"},
                    {"268015829","location-and-object"},
                });

            migrationBuilder.CreateIndex(
                name: "IX_Langtexts_GameApiVersion",
                table: "Langtexts",
                column: "GameApiVersion");

            migrationBuilder.CreateIndex(
                name: "IX_Langtexts_IdType",
                table: "Langtexts",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_LangtextReview_GameApiVersion",
                table: "LangtextReview",
                column: "GameApiVersion");

            migrationBuilder.CreateIndex(
                name: "IX_LangtextReview_IdType",
                table: "LangtextReview",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_LangtextArchive_GameApiVersion",
                table: "LangtextArchive",
                column: "GameApiVersion");

            migrationBuilder.CreateIndex(
                name: "IX_LangtextArchive_IdType",
                table: "LangtextArchive",
                column: "IdType");

            migrationBuilder.AddForeignKey(
                name: "FK_LangtextArchive_GameVersion_GameApiVersion",
                table: "LangtextArchive",
                column: "GameApiVersion",
                principalTable: "GameVersion",
                principalColumn: "GameApiVersion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LangtextArchive_LangTypeCatalog_IdType",
                table: "LangtextArchive",
                column: "IdType",
                principalTable: "LangTypeCatalog",
                principalColumn: "IdType",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LangtextReview_GameVersion_GameApiVersion",
                table: "LangtextReview",
                column: "GameApiVersion",
                principalTable: "GameVersion",
                principalColumn: "GameApiVersion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LangtextReview_LangTypeCatalog_IdType",
                table: "LangtextReview",
                column: "IdType",
                principalTable: "LangTypeCatalog",
                principalColumn: "IdType",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Langtexts_GameVersion_GameApiVersion",
                table: "Langtexts",
                column: "GameApiVersion",
                principalTable: "GameVersion",
                principalColumn: "GameApiVersion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Langtexts_LangTypeCatalog_IdType",
                table: "Langtexts",
                column: "IdType",
                principalTable: "LangTypeCatalog",
                principalColumn: "IdType",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LangtextArchive_GameVersion_GameApiVersion",
                table: "LangtextArchive");

            migrationBuilder.DropForeignKey(
                name: "FK_LangtextArchive_LangTypeCatalog_IdType",
                table: "LangtextArchive");

            migrationBuilder.DropForeignKey(
                name: "FK_LangtextReview_GameVersion_GameApiVersion",
                table: "LangtextReview");

            migrationBuilder.DropForeignKey(
                name: "FK_LangtextReview_LangTypeCatalog_IdType",
                table: "LangtextReview");

            migrationBuilder.DropForeignKey(
                name: "FK_Langtexts_GameVersion_GameApiVersion",
                table: "Langtexts");

            migrationBuilder.DropForeignKey(
                name: "FK_Langtexts_LangTypeCatalog_IdType",
                table: "Langtexts");

            migrationBuilder.DropTable(
                name: "GameVersion");

            migrationBuilder.DropTable(
                name: "LangTypeCatalog");

            migrationBuilder.DropIndex(
                name: "IX_Langtexts_GameApiVersion",
                table: "Langtexts");

            migrationBuilder.DropIndex(
                name: "IX_Langtexts_IdType",
                table: "Langtexts");

            migrationBuilder.DropIndex(
                name: "IX_LangtextReview_GameApiVersion",
                table: "LangtextReview");

            migrationBuilder.DropIndex(
                name: "IX_LangtextReview_IdType",
                table: "LangtextReview");

            migrationBuilder.DropIndex(
                name: "IX_LangtextArchive_GameApiVersion",
                table: "LangtextArchive");

            migrationBuilder.DropIndex(
                name: "IX_LangtextArchive_IdType",
                table: "LangtextArchive");

            migrationBuilder.DropColumn(
                name: "GameApiVersion",
                table: "Langtexts");

            migrationBuilder.DropColumn(
                name: "GameApiVersion",
                table: "LangtextReview");

            migrationBuilder.DropColumn(
                name: "GameApiVersion",
                table: "LangtextArchive");

            migrationBuilder.AddColumn<string>(
                name: "UpdateStats",
                table: "Langtexts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateStats",
                table: "LangtextReview",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateStats",
                table: "LangtextArchive",
                type: "text",
                nullable: true);
        }
    }
}
