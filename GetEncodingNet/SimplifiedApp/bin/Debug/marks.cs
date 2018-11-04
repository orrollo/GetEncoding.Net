public static int MarkBigram(char c1,char c2)
{
  var mark = 0;
  if (c1=='о') // cnt=397435
  {
    if (c2=='в') mark = 41835;
    else if (c2=='н') mark = 37300;
    else if (c2=='с') mark = 35502;
    else if (c2=='т') mark = 35008;
    else if (c2=='л') mark = 32548;
    else if (c2=='р') mark = 31570;
    else if (c2=='м') mark = 26044;
    else if (c2=='г') mark = 24675;
    else if (c2=='д') mark = 24089;
    else if (c2=='й') mark = 18781;
    else if (c2=='б') mark = 17874;
    else if (c2=='е') mark = 12724;
    else if (c2=='ж') mark = 9396;
    else if (c2=='к') mark = 8623;
    else if (c2=='ч') mark = 7719;
    else if (c2=='п') mark = 6302;
    else if (c2=='ш') mark = 6128;
    else if (c2=='з') mark = 5156;
    else if (c2=='и') mark = 4089;
  }
  else if (c1=='е') // cnt=285108
  {
    if (c2=='н') mark = 39330;
    else if (c2=='л') mark = 32157;
    else if (c2=='р') mark = 31675;
    else if (c2=='м') mark = 21000;
    else if (c2=='т') mark = 20706;
    else if (c2=='с') mark = 20490;
    else if (c2=='г') mark = 18258;
    else if (c2=='д') mark = 13807;
    else if (c2=='й') mark = 12947;
    else if (c2=='в') mark = 9733;
    else if (c2=='е') mark = 9554;
    else if (c2=='к') mark = 6666;
    else if (c2=='б') mark = 6239;
    else if (c2=='з') mark = 6142;
    else if (c2=='х') mark = 6126;
    else if (c2=='п') mark = 5601;
    else if (c2=='ж') mark = 4877;
    else if (c2=='ч') mark = 4724;
    else if (c2=='ш') mark = 4171;
    else if (c2=='щ') mark = 3279;
  }
  else if (c1=='а') // cnt=274830
  {
    if (c2=='л') mark = 47797;
    else if (c2=='т') mark = 24555;
    else if (c2=='н') mark = 23025;
    else if (c2=='к') mark = 22261;
    else if (c2=='з') mark = 21334;
    else if (c2=='с') mark = 18917;
    else if (c2=='в') mark = 16876;
    else if (c2=='м') mark = 14280;
    else if (c2=='р') mark = 14279;
    else if (c2=='я') mark = 12731;
    else if (c2=='д') mark = 10415;
    else if (c2=='ж') mark = 5808;
    else if (c2=='е') mark = 5122;
    else if (c2=='ш') mark = 5098;
    else if (c2=='х') mark = 4861;
    else if (c2=='ю') mark = 4227;
    else if (c2=='п') mark = 4168;
    else if (c2=='ч') mark = 4046;
    else if (c2=='г') mark = 3871;
    else if (c2=='б') mark = 3304;
    else if (c2=='й') mark = 3022;
  }
  else if (c1=='н') // cnt=273495
  {
    if (c2=='а') mark = 56249;
    else if (c2=='е') mark = 49594;
    else if (c2=='о') mark = 49227;
    else if (c2=='и') mark = 41031;
    else if (c2=='ы') mark = 16816;
    else if (c2=='н') mark = 14819;
    else if (c2=='у') mark = 11971;
    else if (c2=='я') mark = 10928;
    else if (c2=='ь') mark = 5695;
    else if (c2=='с') mark = 3450;
    else if (c2=='д') mark = 3407;
  }
  else if (c1=='т') // cnt=243618
  {
    if (c2=='о') mark = 77377;
    else if (c2=='ь') mark = 30540;
    else if (c2=='а') mark = 28944;
    else if (c2=='е') mark = 24856;
    else if (c2=='и') mark = 19004;
    else if (c2=='в') mark = 13926;
    else if (c2=='р') mark = 13717;
    else if (c2=='ы') mark = 7581;
    else if (c2=='у') mark = 6956;
    else if (c2=='н') mark = 5320;
    else if (c2=='с') mark = 4529;
    else if (c2=='я') mark = 2763;
  }
  else if (c1=='с') // cnt=225312
  {
    if (c2=='т') mark = 58348;
    else if (c2=='к') mark = 25523;
    else if (c2=='е') mark = 20519;
    else if (c2=='я') mark = 16224;
    else if (c2=='ь') mark = 15149;
    else if (c2=='о') mark = 14260;
    else if (c2=='л') mark = 13528;
    else if (c2=='в') mark = 9478;
    else if (c2=='п') mark = 8983;
    else if (c2=='а') mark = 8153;
    else if (c2=='и') mark = 7708;
    else if (c2=='н') mark = 5482;
    else if (c2=='м') mark = 4763;
    else if (c2=='с') mark = 3889;
    else if (c2=='у') mark = 3866;
  }
  else if (c1=='и') // cnt=201465
  {
    if (c2=='л') mark = 24084;
    else if (c2=='т') mark = 18499;
    else if (c2=='н') mark = 17993;
    else if (c2=='м') mark = 15247;
    else if (c2=='с') mark = 13837;
    else if (c2=='е') mark = 13623;
    else if (c2=='в') mark = 13472;
    else if (c2=='з') mark = 10002;
    else if (c2=='к') mark = 9556;
    else if (c2=='я') mark = 9298;
    else if (c2=='х') mark = 8383;
    else if (c2=='д') mark = 8319;
    else if (c2=='ч') mark = 7562;
    else if (c2=='й') mark = 6550;
    else if (c2=='ц') mark = 5337;
    else if (c2=='и') mark = 3775;
    else if (c2=='р') mark = 2972;
    else if (c2=='г') mark = 2404;
    else if (c2=='б') mark = 2079;
  }
  else if (c1=='л') // cnt=187013
  {
    if (c2=='а') mark = 37334;
    else if (c2=='о') mark = 31906;
    else if (c2=='и') mark = 31150;
    else if (c2=='е') mark = 23132;
    else if (c2=='ь') mark = 19508;
    else if (c2=='я') mark = 9356;
    else if (c2=='ю') mark = 7416;
    else if (c2=='с') mark = 6292;
    else if (c2=='у') mark = 6216;
    else if (c2=='ы') mark = 5075;
    else if (c2=='к') mark = 2207;
    else if (c2=='ж') mark = 1994;
  }
  else if (c1=='р') // cnt=185082
  {
    if (c2=='а') mark = 39903;
    else if (c2=='о') mark = 35807;
    else if (c2=='е') mark = 30367;
    else if (c2=='и') mark = 25757;
    else if (c2=='у') mark = 13603;
    else if (c2=='ы') mark = 7714;
    else if (c2=='я') mark = 4967;
    else if (c2=='ь') mark = 4879;
    else if (c2=='н') mark = 3436;
    else if (c2=='т') mark = 2773;
  }
  else if (c1=='в') // cnt=177126
  {
    if (c2=='о') mark = 38754;
    else if (c2=='а') mark = 29546;
    else if (c2=='е') mark = 23194;
    else if (c2=='и') mark = 18807;
    else if (c2=='с') mark = 17061;
    else if (c2=='ы') mark = 13026;
    else if (c2=='н') mark = 6328;
    else if (c2=='ш') mark = 6036;
    else if (c2=='л') mark = 5054;
    else if (c2=='у') mark = 4196;
    else if (c2=='р') mark = 3920;
    else if (c2=='з') mark = 2625;
  }
  else if (c1=='д') // cnt=131673
  {
    if (c2=='е') mark = 24245;
    else if (c2=='а') mark = 22822;
    else if (c2=='о') mark = 21171;
    else if (c2=='и') mark = 12174;
    else if (c2=='у') mark = 8903;
    else if (c2=='н') mark = 8576;
    else if (c2=='р') mark = 8292;
    else if (c2=='в') mark = 4920;
    else if (c2=='л') mark = 4138;
    else if (c2=='ы') mark = 2985;
    else if (c2=='ь') mark = 2759;
    else if (c2=='я') mark = 2234;
    else if (c2=='с') mark = 2069;
  }
  else if (c1=='к') // cnt=127586
  {
    if (c2=='о') mark = 42600;
    else if (c2=='а') mark = 40252;
    else if (c2=='и') mark = 13405;
    else if (c2=='р') mark = 7965;
    else if (c2=='у') mark = 7686;
    else if (c2=='н') mark = 5385;
    else if (c2=='е') mark = 2441;
    else if (c2=='т') mark = 2187;
    else if (c2=='л') mark = 2002;
    else if (c2=='с') mark = 1880;
    else if (c2=='в') mark = 1353;
  }
  else if (c1=='п') // cnt=114286
  {
    if (c2=='о') mark = 44621;
    else if (c2=='р') mark = 33318;
    else if (c2=='е') mark = 12390;
    else if (c2=='а') mark = 6119;
    else if (c2=='и') mark = 4216;
    else if (c2=='л') mark = 3183;
    else if (c2=='у') mark = 3129;
    else if (c2=='ь') mark = 2400;
    else if (c2=='я') mark = 1792;
    else if (c2=='ы') mark = 1341;
  }
  else if (c1=='м') // cnt=92828
  {
    if (c2=='о') mark = 17467;
    else if (c2=='и') mark = 16382;
    else if (c2=='е') mark = 15141;
    else if (c2=='а') mark = 14035;
    else if (c2=='у') mark = 13271;
    else if (c2=='н') mark = 6075;
    else if (c2=='ы') mark = 4490;
    else if (c2=='я') mark = 2261;
  }
  else if (c1=='у') // cnt=87774
  {
    if (c2=='д') mark = 8769;
    else if (c2=='ж') mark = 7719;
    else if (c2=='т') mark = 6294;
    else if (c2=='с') mark = 6143;
    else if (c2=='л') mark = 6053;
    else if (c2=='г') mark = 5925;
    else if (c2=='м') mark = 5494;
    else if (c2=='ю') mark = 5161;
    else if (c2=='в') mark = 4972;
    else if (c2=='ч') mark = 4256;
    else if (c2=='к') mark = 3974;
    else if (c2=='ш') mark = 3861;
    else if (c2=='п') mark = 3367;
    else if (c2=='р') mark = 3078;
    else if (c2=='б') mark = 2971;
    else if (c2=='з') mark = 2911;
    else if (c2=='х') mark = 1716;
    else if (c2=='н') mark = 1299;
    else if (c2=='щ') mark = 1293;
    else if (c2=='е') mark = 1135;
  }
  else if (c1=='г') // cnt=80259
  {
    if (c2=='о') mark = 45508;
    else if (c2=='л') mark = 7732;
    else if (c2=='д') mark = 5710;
    else if (c2=='р') mark = 4975;
    else if (c2=='а') mark = 4660;
    else if (c2=='и') mark = 3876;
    else if (c2=='у') mark = 3252;
    else if (c2=='е') mark = 2281;
    else if (c2=='н') mark = 1136;
  }
  else if (c1=='б') // cnt=76117
  {
    if (c2=='ы') mark = 21470;
    else if (c2=='о') mark = 11121;
    else if (c2=='е') mark = 10803;
    else if (c2=='р') mark = 5914;
    else if (c2=='у') mark = 5150;
    else if (c2=='а') mark = 4810;
    else if (c2=='л') mark = 4161;
    else if (c2=='и') mark = 3866;
    else if (c2=='я') mark = 2416;
    else if (c2=='н') mark = 1484;
    else if (c2=='щ') mark = 1118;
    else if (c2=='к') mark = 1107;
  }
  else if (c1=='з') // cnt=68548
  {
    if (c2=='а') mark = 28002;
    else if (c2=='н') mark = 9329;
    else if (c2=='в') mark = 4098;
    else if (c2=='д') mark = 3687;
    else if (c2=='о') mark = 2720;
    else if (c2=='ы') mark = 2485;
    else if (c2=='я') mark = 2386;
    else if (c2=='г') mark = 2227;
    else if (c2=='ь') mark = 1958;
    else if (c2=='и') mark = 1768;
    else if (c2=='м') mark = 1436;
    else if (c2=='е') mark = 1425;
    else if (c2=='у') mark = 1380;
    else if (c2=='р') mark = 1099;
    else if (c2=='л') mark = 1063;
    else if (c2=='б') mark = 871;
    else if (c2=='ж') mark = 715;
  }
  else if (c1=='ч') // cnt=67126
  {
    if (c2=='т') mark = 20885;
    else if (c2=='е') mark = 17302;
    else if (c2=='а') mark = 11019;
    else if (c2=='и') mark = 7636;
    else if (c2=='у') mark = 3725;
    else if (c2=='н') mark = 2714;
    else if (c2=='к') mark = 1516;
    else if (c2=='ь') mark = 933;
  }
  else if (c1=='ы') // cnt=63399
  {
    if (c2=='л') mark = 13263;
    else if (c2=='м') mark = 7546;
    else if (c2=='й') mark = 7125;
    else if (c2=='е') mark = 5722;
    else if (c2=='в') mark = 5435;
    else if (c2=='х') mark = 5298;
    else if (c2=='с') mark = 3957;
    else if (c2=='т') mark = 3522;
    else if (c2=='ш') mark = 2522;
    else if (c2=='б') mark = 2025;
    else if (c2=='н') mark = 1484;
    else if (c2=='р') mark = 1474;
    else if (c2=='ч') mark = 910;
    else if (c2=='к') mark = 865;
  }
  else if (c1=='ж') // cnt=47952
  {
    if (c2=='е') mark = 20420;
    else if (c2=='и') mark = 7547;
    else if (c2=='а') mark = 7281;
    else if (c2=='н') mark = 5851;
    else if (c2=='д') mark = 3992;
    else if (c2=='у') mark = 989;
  }
  else if (c1=='ш') // cnt=40347
  {
    if (c2=='е') mark = 12455;
    else if (c2=='и') mark = 9180;
    else if (c2=='а') mark = 6453;
    else if (c2=='л') mark = 2597;
    else if (c2=='к') mark = 2054;
    else if (c2=='н') mark = 1963;
    else if (c2=='ь') mark = 1763;
    else if (c2=='у') mark = 1589;
    else if (c2=='о') mark = 1512;
  }
  else if (c1=='я') // cnt=38749
  {
    if (c2=='т') mark = 7298;
    else if (c2=='л') mark = 4531;
    else if (c2=='с') mark = 4010;
    else if (c2=='з') mark = 3364;
    else if (c2=='н') mark = 3292;
    else if (c2=='д') mark = 3030;
    else if (c2=='ж') mark = 2079;
    else if (c2=='м') mark = 1915;
    else if (c2=='в') mark = 1474;
    else if (c2=='щ') mark = 1115;
    else if (c2=='х') mark = 1003;
    else if (c2=='г') mark = 946;
    else if (c2=='я') mark = 796;
    else if (c2=='к') mark = 706;
    else if (c2=='ч') mark = 650;
    else if (c2=='ю') mark = 581;
    else if (c2=='е') mark = 491;
  }
  else if (c1=='ь') // cnt=33987
  {
    if (c2=='к') mark = 6030;
    else if (c2=='н') mark = 5979;
    else if (c2=='е') mark = 4628;
    else if (c2=='с') mark = 4407;
    else if (c2=='я') mark = 2567;
    else if (c2=='ш') mark = 2408;
    else if (c2=='ю') mark = 2138;
    else if (c2=='м') mark = 1400;
    else if (c2=='и') mark = 1093;
    else if (c2=='з') mark = 796;
    else if (c2=='ц') mark = 561;
    else if (c2=='б') mark = 488;
    else if (c2=='т') mark = 386;
    else if (c2=='ч') mark = 350;
  }
  else if (c1=='х') // cnt=21665
  {
    if (c2=='о') mark = 11297;
    else if (c2=='а') mark = 4305;
    else if (c2=='л') mark = 2123;
    else if (c2=='и') mark = 724;
    else if (c2=='у') mark = 700;
    else if (c2=='в') mark = 595;
    else if (c2=='н') mark = 534;
    else if (c2=='р') mark = 429;
    else if (c2=='с') mark = 290;
    else if (c2=='м') mark = 280;
  }
  else if (c1=='э') // cnt=14480
  {
    if (c2=='т') mark = 13457;
    else if (c2=='л') mark = 305;
    else if (c2=='к') mark = 230;
  }
  else if (c1=='щ') // cnt=13682
  {
    if (c2=='е') mark = 6925;
    else if (c2=='и') mark = 4088;
    else if (c2=='а') mark = 1895;
    else if (c2=='у') mark = 455;
    else if (c2=='н') mark = 191;
  }
  else if (c1=='ц') // cnt=13353
  {
    if (c2=='е') mark = 4186;
    else if (c2=='а') mark = 2953;
    else if (c2=='у') mark = 1874;
    else if (c2=='о') mark = 1670;
    else if (c2=='и') mark = 1197;
    else if (c2=='ы') mark = 775;
    else if (c2=='к') mark = 477;
    else if (c2=='в') mark = 150;
  }
  else if (c1=='ю') // cnt=13314
  {
    if (c2=='д') mark = 3748;
    else if (c2=='б') mark = 2497;
    else if (c2=='щ') mark = 2299;
    else if (c2=='т') mark = 1899;
    else if (c2=='с') mark = 627;
    else if (c2=='ш') mark = 494;
    else if (c2=='ч') mark = 404;
    else if (c2=='р') mark = 349;
    else if (c2=='ю') mark = 184;
    else if (c2=='н') mark = 160;
    else if (c2=='л') mark = 151;
    else if (c2=='ц') mark = 145;
  }
  else if (c1=='й') // cnt=8861
  {
    if (c2=='с') mark = 3044;
    else if (c2=='н') mark = 1705;
    else if (c2=='т') mark = 1179;
    else if (c2=='д') mark = 922;
    else if (c2=='ш') mark = 433;
    else if (c2=='л') mark = 359;
    else if (c2=='ч') mark = 339;
    else if (c2=='к') mark = 334;
    else if (c2=='м') mark = 219;
    else if (c2=='ц') mark = 200;
  }
  else if (c1=='ф') // cnt=6690
  {
    if (c2=='и') mark = 2275;
    else if (c2=='р') mark = 1702;
    else if (c2=='е') mark = 755;
    else if (c2=='а') mark = 637;
    else if (c2=='о') mark = 446;
    else if (c2=='у') mark = 282;
    else if (c2=='л') mark = 225;
    else if (c2=='ь') mark = 129;
    else if (c2=='т') mark = 103;
  }
  else if (c1=='ъ') // cnt=1603
  {
    if (c2=='е') mark = 760;
    else if (c2=='я') mark = 545;
    else if (c2=='ю') mark = 296;
  }
  return mark;
}
