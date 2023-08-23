using System;
using System.Collections.Generic;
using System.Text;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class CountryCodes : IProcessor
    {
        public CountryCodes()
        {
           // airportsIndex = SortedSearch.CreateIndex(data.Select((t, i) => (t, i)), a => GetFields(a));
        }
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("US", "Country Info"));
            }

            var input = query.OriginalInputTrim;
            if (string.Equals(input, "color", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (input.IsSingleWord())
            {
               return Country(input);
                
            }

            return null;
        }

        private object Country(string input)
        {
            var result = new List<object>();
            foreach(var country in countryData)
            {
                if(country.displayName.ToUpperInvariant().Equals(input.ToUpperInvariant()))
                {
                    var name = country.displayName +","+ country.regionName;
                    result.Add(SectionHeader(name));



                    var officialName = country.officialName;
                    var capital = country.capital;
                    var TLD = country.TLD;
                    var languages = country.languages;
                    var currencyName = country.currencyName;
                    var currencyAlphabeticCode = country.currencyAlphabeticCode;
                    var ISO3166Alpha3 = country.ISO3166Alpha3;
                    var ISO3166Numeric = country.ISO3166Numeric;
                    var ISO3166Alpha2 = country.ISO3166Alpha2;
                    var geonameID = country.geonameID;

                    var pairs = new List<(string, string)>
            {
                ("officialName:", country.officialName),
                ("capital:",  country.capital),
                ("TLD:", country.TLD),
                ("currencyName:", country.currencyName),
                ("currencyAlphabeticCode:", country.currencyAlphabeticCode),
                ("ISO3166Alpha3:", country.ISO3166Alpha3),
                ("ISO3166Numeric:", country.ISO3166Numeric),
                ("ISO3166Alpha2:", country.ISO3166Alpha2),
                ("geonameID:", country.geonameID)

            };
                    
                    result.Add(NameValueTable(null, right =>
                    {
                        right.Style = NodeStyles.Fixed;
                       
                    }, entries: pairs.ToArray()));

                    //result.Add(Fixed("Capital"));
                    //result.Add(capital);

                }
            }

            return result.Count > 0 ? result : null;

        }

        private static readonly (string CallingCode, string displayName, string officialName, string regionName, string subregionName, string capital, string continent, string TLD, string languages, string currencyName, string currencyAlphabeticCode, string ISO3166Alpha3, string ISO3166Numeric, string ISO3166Alpha2, string geonameID)[] countryData =
        {
          ("886","Taiwan","Taiwan","Asia","Eastern Asia","Taipei","AS",".tw","zh-TW,zh,nan,hak","New Taiwan dollar","TWD","TWN","158","TW","1668284"),
("93","Afghanistan","Afghanistan","Asia","Southern Asia'","Kabul","AS",".af","fa-AF,ps,uz-AF,tk","Afghani","AFN","AFG","4","AF","1149361"),
("355","Albania","Albania","Europe","Southern Europe","Tirana","EU",".al","sq,el","Lek","ALL","ALB","8","AL","783754"),
("213","Algeria","Algeria","Africa","Northern Africa","Algiers","AF",".dz","ar-DZ","Algerian Dinar","DZD","DZA","12","DZ","2589581"),
("1-684","American Samoa","American Samoa","Oceania","Polynesia","Pago Pago","OC",".as","en-AS,sm,to","US Dollar","USD","ASM","16","AS","5880801"),
("376","Andorra","Andorra","Europe","Southern Europe","Andorra la Vella","EU",".ad","ca","Euro","EUR","AND","20","AD","3041565"),
("244","Angola","Angola","Africa","Sub-Saharan Africa","Luanda","AF",".ao","pt-AO","Kwanza","AOA","AGO","24","AO","3351879"),
("1-264","Anguilla","Anguilla","Americas","Latin America and the Caribbean","The Valley","NA",".ai","en-AI","East Caribbean Dollar","XCD","AIA","660","AI","3573511"),
("672","Antarctica","Antarctica","","","","AN",".aq","","No universal currency","","ATA","10","AQ","6697173"),
("1-268","Antigua & Barbuda","Antigua and Barbuda","Americas","Latin America and the Caribbean","St. John's","NA",".ag","en-AG","East Caribbean Dollar","XCD","ATG","28","AG","3576396"),
("54","Argentina","Argentina","Americas","Latin America and the Caribbean","Buenos Aires","SA",".ar","es-AR,en,it,de,fr,gn","Argentine Peso","ARS","ARG","32","AR","3865483"),
("374","Armenia","Armenia","Asia","Western Asia","Yerevan","AS",".am","hy","Armenian Dram","AMD","ARM","51","AM","174982"),
("297","Aruba","Aruba","Americas","Latin America and the Caribbean","Oranjestad","NA",".aw","nl-AW,pap,es,en","Aruban Florin","AWG","ABW","533","AW","3577279"),
("61","Australia","Australia","Oceania","Australia and New Zealand","Canberra","OC",".au","en-AU","Australian Dollar","AUD","AUS","36","AU","2077456"),
("43","Austria","Austria","Europe","Western Europe","Vienna","EU",".at","de-AT,hr,hu,sl","Euro","EUR","AUT","40","AT","2782113"),
("994","Azerbaijan","Azerbaijan","Asia","Western Asia","Baku","AS",".az","az,ru,hy","Azerbaijan Manat","AZN","AZE","31","AZ","587116"),
("1-242","Bahamas","Bahamas","Americas","Latin America and the Caribbean","Nassau","NA",".bs","en-BS","Bahamian Dollar","BSD","BHS","44","BS","3572887"),
("973","Bahrain","Bahrain","Asia","Western Asia","Manama","AS",".bh","ar-BH,en,fa,ur","Bahraini Dinar","BHD","BHR","48","BH","290291"),
("880","Bangladesh","Bangladesh","Asia","Southern Asia","Dhaka","AS",".bd","bn-BD,en","Taka","BDT","BGD","50","BD","1210997"),
("1-246","Barbados","Barbados","Americas","Latin America and the Caribbean","Bridgetown","NA",".bb","en-BB","Barbados Dollar","BBD","BRB","52","BB","3374084"),
("375","Belarus","Belarus","Europe","Eastern Europe","Minsk","EU",".by","be,ru","Belarusian Ruble","BYN","BLR","112","BY","630336"),
("32","Belgium","Belgium","Europe","Western Europe","Brussels","EU",".be","nl-BE,fr-BE,de-BE","Euro","EUR","BEL","56","BE","2802361"),
("501","Belize","Belize","Americas","Latin America and the Caribbean","Belmopan","NA",".bz","en-BZ,es","Belize Dollar","BZD","BLZ","84","BZ","3582678"),
("229","Benin","Benin","Africa","Sub-Saharan Africa","Porto-Novo","AF",".bj","fr-BJ","CFA Franc BCEAO","XOF","BEN","204","BJ","2395170"),
("1-441","Bermuda","Bermuda","Americas","Northern America","Hamilton","NA",".bm","en-BM,pt","Bermudian Dollar","BMD","BMU","60","BM","3573345"),
("975","Bhutan","Bhutan","Asia","Southern Asia","Thimphu","AS",".bt","dz","Indian Rupee,Ngultrum","INR,BTN","BTN","64","BT","1252634"),
("591","Bolivia","Bolivia (Plurinational State of)","Americas","Latin America and the Caribbean","Sucre","SA",".bo","es-BO,qu,ay","Boliviano","BOB","BOL","68","BO","3923057"),
("599","Caribbean Netherlands","Bonaire, Sint Eustatius and Saba","Americas","Latin America and the Caribbean","","NA",".bq","nl,pap,en","US Dollar","USD","BES","535","BQ","7626844"),
("387","Bosnia","Bosnia and Herzegovina","Europe","Southern Europe","Sarajevo","EU",".ba","bs,hr-BA,sr-BA","Convertible Mark","BAM","BIH","70","BA","3277605"),
("267","Botswana","Botswana","Africa","Sub-Saharan Africa","Gaborone","AF",".bw","en-BW,tn-BW","Pula","BWP","BWA","72","BW","933860"),
("47","Bouvet Island","Bouvet Island","Americas","Latin America and the Caribbean","","AN",".bv","","Norwegian Krone","NOK","BVT","74","BV","3371123"),
("55","Brazil","Brazil","Americas","Latin America and the Caribbean","Brasilia","SA",".br","pt-BR,es,en,fr","Brazilian Real","BRL","BRA","76","BR","3469034"),
("246","British Indian Ocean Territory","British Indian Ocean Territory","Africa","Sub-Saharan Africa","Diego Garcia","AS",".io","en-IO","US Dollar","USD","IOT","86","IO","1282588"),
("1-284","British Virgin Islands","British Virgin Islands","Americas","Latin America and the Caribbean","Road Town","NA",".vg","en-VG","US Dollar","USD","VGB","92","VG","3577718"),
("673","Brunei","Brunei Darussalam","Asia","South-eastern Asia","Bandar Seri Begawan","AS",".bn","ms-BN,en-BN","Brunei Dollar","BND","BRN","96","BN","1820814"),
("359","Bulgaria","Bulgaria","Europe","Eastern Europe","Sofia","EU",".bg","bg,tr-BG,rom","Bulgarian Lev","BGN","BGR","100","BG","732800"),
("226","Burkina Faso","Burkina Faso","Africa","Sub-Saharan Africa","Ouagadougou","AF",".bf","fr-BF,mos","CFA Franc BCEAO","XOF","BFA","854","BF","2361809"),
("257","Burundi","Burundi","Africa","Sub-Saharan Africa","Bujumbura","AF",".bi","fr-BI,rn","Burundi Franc","BIF","BDI","108","BI","433561"),
("238","Cape Verde","Cabo Verde","Africa","Sub-Saharan Africa","Praia","AF",".cv","pt-CV","Cabo Verde Escudo","CVE","CPV","132","CV","3374766"),
("855","Cambodia","Cambodia","Asia","South-eastern Asia","Phnom Penh","AS",".kh","km,fr,en","Riel","KHR","KHM","116","KH","1831722"),
("237","Cameroon","Cameroon","Africa","Sub-Saharan Africa","Yaounde","AF",".cm","en-CM,fr-CM","CFA Franc BEAC","XAF","CMR","120","CM","2233387"),
("1","Canada","Canada","Americas","Northern America","Ottawa","NA",".ca","en-CA,fr-CA,iu","Canadian Dollar","CAD","CAN","124","CA","6251999"),
("1-345","Cayman Islands","Cayman Islands","Americas","Latin America and the Caribbean","George Town","NA",".ky","en-KY","Cayman Islands Dollar","KYD","CYM","136","KY","3580718"),
("236","Central African Republic","Central African Republic","Africa","Sub-Saharan Africa","Bangui","AF",".cf","fr-CF,sg,ln,kg","CFA Franc BEAC","XAF","CAF","140","CF","239880"),
("235","Chad","Chad","Africa","Sub-Saharan Africa","N'Djamena","AF",".td","fr-TD,ar-TD,sre","CFA Franc BEAC","XAF","TCD","148","TD","2434508"),
("56","Chile","Chile","Americas","Latin America and the Caribbean","Santiago","SA",".cl","es-CL","Chilean Peso","CLP","CHL","152","CL","3895114"),
("86","China","China","Asia","Eastern Asia","Beijing","AS",".cn","zh-CN,yue,wuu,dta,ug,za","Yuan Renminbi","CNY","CHN","156","CN","1814991"),
("852","Hong Kong","China, Hong Kong Special Administrative Region","Asia","Eastern Asia","Hong Kong","AS",".hk","zh-HK,yue,zh,en","Hong Kong Dollar","HKD","HKG","344","HK","1819730"),
("853","Macau","China, Macao Special Administrative Region","Asia","Eastern Asia","Macao","AS",".mo","zh,zh-MO,pt","Pataca","MOP","MAC","446","MO","1821275"),
("61","Christmas Island","Christmas Island","Oceania","Australia and New Zealand","Flying Fish Cove","OC",".cx","en,zh,ms-CC","Australian Dollar","AUD","CXR","162","CX","2078138"),
("61","Cocos (Keeling) Islands","Cocos (Keeling) Islands","Oceania","Australia and New Zealand","West Island","AS",".cc","ms-CC,en","Australian Dollar","AUD","CCK","166","CC","1547376"),
("57","Colombia","Colombia","Americas","Latin America and the Caribbean","Bogota","SA",".co","es-CO","Colombian Peso","COP","COL","170","CO","3686110"),
("269","Comoros","Comoros","Africa","Sub-Saharan Africa","Moroni","AF",".km","ar,fr-KM","Comorian Franc ","KMF","COM","174","KM","921929"),
("242","Congo - Brazzaville","Congo","Africa","Sub-Saharan Africa","Brazzaville","AF",".cg","fr-CG,kg,ln-CG","CFA Franc BEAC","XAF","COG","178","CG","2260494"),
("682","Cook Islands","Cook Islands","Oceania","Polynesia","Avarua","OC",".ck","en-CK,mi","New Zealand Dollar","NZD","COK","184","CK","1899402"),
("506","Costa Rica","Costa Rica","Americas","Latin America and the Caribbean","San Jose","NA",".cr","es-CR,en","Costa Rican Colon","CRC","CRI","188","CR","3624060"),
("385","Croatia","Croatia","Europe","Southern Europe","Zagreb","EU",".hr","hr-HR,sr","Kuna","HRK","HRV","191","HR","3202326"),
("53","Cuba","Cuba","Americas","Latin America and the Caribbean","Havana","NA",".cu","es-CU,pap","Cuban Peso,Peso Convertible","CUP,CUC","CUB","192","CU","3562981"),
("599","CuraÃ§ao","CuraÃ§ao","Americas","Latin America and the Caribbean"," Willemstad","NA",".cw","nl,pap","Netherlands Antillean Guilder","ANG","CUW","531","CW","7626836"),
("357","Cyprus","Cyprus","Asia","Western Asia","Nicosia","EU",".cy","el-CY,tr-CY,en","Euro","EUR","CYP","196","CY","146669"),
("420","Czechia","Czechia","Europe","Eastern Europe","Prague","EU",".cz","cs,sk","Czech Koruna","CZK","CZE","203","CZ","3077311"),
("225","CÃ´te dâ€™Ivoire","CÃ´te d'Ivoire","Africa","Sub-Saharan Africa","Yamoussoukro","AF",".ci","fr-CI","CFA Franc BCEAO","XOF","CIV","384","CI","2287781"),
("850","North Korea","Democratic People's Republic of Korea","Asia","Eastern Asia","Pyongyang","AS",".kp","ko-KP","North Korean Won","KPW","PRK","408","KP","1873107"),
("243","Congo - Kinshasa","Democratic Republic of the Congo","Africa","Sub-Saharan Africa","Kinshasa","AF",".cd","fr-CD,ln,ktu,kg,sw,lua","Congolese Franc","CDF","COD","180","CD","203312"),
("45","Denmark","Denmark","Europe","Northern Europe","Copenhagen","EU",".dk","da-DK,en,fo,de-DK","Danish Krone","DKK","DNK","208","DK","2623032"),
("253","Djibouti","Djibouti","Africa","Sub-Saharan Africa","Djibouti","AF",".dj","fr-DJ,ar,so-DJ,aa","Djibouti Franc","DJF","DJI","262","DJ","223816"),
("1-767","Dominica","Dominica","Americas","Latin America and the Caribbean","Roseau","NA",".dm","en-DM","East Caribbean Dollar","XCD","DMA","212","DM","3575830"),
("1-809,1-829,1-849","Dominican Republic","Dominican Republic","Americas","Latin America and the Caribbean","Santo Domingo","NA",".do","es-DO","Dominican Peso","DOP","DOM","214","DO","3508796"),
("593","Ecuador","Ecuador","Americas","Latin America and the Caribbean","Quito","SA",".ec","es-EC","US Dollar","USD","ECU","218","EC","3658394"),
("20","Egypt","Egypt","Africa","Northern Africa","Cairo","AF",".eg","ar-EG,en,fr","Egyptian Pound","EGP","EGY","818","EG","357994"),
("503","El Salvador","El Salvador","Americas","Latin America and the Caribbean","San Salvador","NA",".sv","es-SV","El Salvador Colon,US Dollar","SVC,USD","SLV","222","SV","3585968"),
("240","Equatorial Guinea","Equatorial Guinea","Africa","Sub-Saharan Africa","Malabo","AF",".gq","es-GQ,fr","CFA Franc BEAC","XAF","GNQ","226","GQ","2309096"),
("291","Eritrea","Eritrea","Africa","Sub-Saharan Africa","Asmara","AF",".er","aa-ER,ar,tig,kun,ti-ER","Nakfa","ERN","ERI","232","ER","338010"),
("372","Estonia","Estonia","Europe","Northern Europe","Tallinn","EU",".ee","et,ru","Euro","EUR","EST","233","EE","453733"),
("268","Eswatini","Eswatini","Africa","Sub-Saharan Africa","Mbabane","AF",".sz","en-SZ,ss-SZ","Lilangeni","SZL","SWZ","748","SZ","934841"),
("251","Ethiopia","Ethiopia","Africa","Sub-Saharan Africa","Addis Ababa","AF",".et","am,en-ET,om-ET,ti-ET,so-ET,sid","Ethiopian Birr","ETB","ETH","231","ET","337996"),
("500","Falkland Islands","Falkland Islands (Malvinas)","Americas","Latin America and the Caribbean","Stanley","SA",".fk","en-FK","","","FLK","238","FK","3474414"),
("298","Faroe Islands","Faroe Islands","Europe","Northern Europe","Torshavn","EU",".fo","fo,da-FO","Danish Krone","DKK","FRO","234","FO","2622320"),
("679","Fiji","Fiji","Oceania","Melanesia","Suva","OC",".fj","en-FJ,fj","Fiji Dollar","FJD","FJI","242","FJ","2205218"),
("358","Finland","Finland","Europe","Northern Europe","Helsinki","EU",".fi","fi-FI,sv-FI,smn","Euro","EUR","FIN","246","FI","660013"),
("33","France","France","Europe","Western Europe","Paris","EU",".fr","fr-FR,frp,br,co,ca,eu,oc","Euro","EUR","FRA","250","FR","3017382"),
("594","French Guiana","French Guiana","Americas","Latin America and the Caribbean","Cayenne","SA",".gf","fr-GF","Euro","EUR","GUF","254","GF","3381670"),
("689","French Polynesia","French Polynesia","Oceania","Polynesia","Papeete","OC",".pf","fr-PF,ty","CFP Franc","XPF","PYF","258","PF","4030656"),
("262","French Southern Territories","French Southern Territories","Africa","Sub-Saharan Africa","Port-aux-Francais","AN",".tf","fr","Euro","EUR","ATF","260","TF","1546748"),
("241","Gabon","Gabon","Africa","Sub-Saharan Africa","Libreville","AF",".ga","fr-GA","CFA Franc BEAC","XAF","GAB","266","GA","2400553"),
("220","Gambia","Gambia","Africa","Sub-Saharan Africa","Banjul","AF",".gm","en-GM,mnk,wof,wo,ff","Dalasi","GMD","GMB","270","GM","2413451"),
("995","Georgia","Georgia","Asia","Western Asia","Tbilisi","AS",".ge","ka,ru,hy,az","Lari","GEL","GEO","268","GE","614540"),
("49","Germany","Germany","Europe","Western Europe","Berlin","EU",".de","de","Euro","EUR","DEU","276","DE","2921044"),
("233","Ghana","Ghana","Africa","Sub-Saharan Africa","Accra","AF",".gh","en-GH,ak,ee,tw","Ghana Cedi","GHS","GHA","288","GH","2300660"),
("350","Gibraltar","Gibraltar","Europe","Southern Europe","Gibraltar","EU",".gi","en-GI,es,it,pt","Gibraltar Pound","GIP","GIB","292","GI","2411586"),
("30","Greece","Greece","Europe","Southern Europe","Athens","EU",".gr","el-GR,en,fr","Euro","EUR","GRC","300","GR","390903"),
("299","Greenland","Greenland","Americas","Northern America","Nuuk","NA",".gl","kl,da-GL,en","Danish Krone","DKK","GRL","304","GL","3425505"),
("1-473","Grenada","Grenada","Americas","Latin America and the Caribbean","St. George's","NA",".gd","en-GD","East Caribbean Dollar","XCD","GRD","308","GD","3580239"),
("590","Guadeloupe","Guadeloupe","Americas","Latin America and the Caribbean","Basse-Terre","NA",".gp","fr-GP","Euro","EUR","GLP","312","GP","3579143"),
("1-671","Guam","Guam","Oceania","Micronesia","Hagatna","OC",".gu","en-GU,ch-GU","US Dollar","USD","GUM","316","GU","4043988"),
("502","Guatemala","Guatemala","Americas","Latin America and the Caribbean","Guatemala City","NA",".gt","es-GT","Quetzal","GTQ","GTM","320","GT","3595528"),
("44","Guernsey","Guernsey","Europe","Northern Europe","St Peter Port","EU",".gg","en,nrf","Pound Sterling","GBP","GGY","831","GG","3042362"),
("224","Guinea","Guinea","Africa","Sub-Saharan Africa","Conakry","AF",".gn","fr-GN","Guinean Franc","GNF","GIN","324","GN","2420477"),
("245","Guinea-Bissau","Guinea-Bissau","Africa","Sub-Saharan Africa","Bissau","AF",".gw","pt-GW,pov","CFA Franc BCEAO","XOF","GNB","624","GW","2372248"),
("592","Guyana","Guyana","Americas","Latin America and the Caribbean","Georgetown","SA",".gy","en-GY","Guyana Dollar","GYD","GUY","328","GY","3378535"),
("509","Haiti","Haiti","Americas","Latin America and the Caribbean","Port-au-Prince","NA",".ht","ht,fr-HT","Gourde,US Dollar","HTG,USD","HTI","332","HT","3723988"),
("672","Heard & McDonald Islands","Heard Island and McDonald Islands","Oceania","Australia and New Zealand","","AN",".hm","","Australian Dollar","AUD","HMD","334","HM","1547314"),
("39-06","Vatican City","Holy See","Europe","Southern Europe","Vatican City","EU",".va","la,it,fr","Euro","EUR","VAT","336","VA","3164670"),
("504","Honduras","Honduras","Americas","Latin America and the Caribbean","Tegucigalpa","NA",".hn","es-HN,cab,miq","Lempira","HNL","HND","340","HN","3608932"),
("36","Hungary","Hungary","Europe","Eastern Europe","Budapest","EU",".hu","hu-HU","Forint","HUF","HUN","348","HU","719819"),
("354","Iceland","Iceland","Europe","Northern Europe","Reykjavik","EU",".is","is,en,de,da,sv,no","Iceland Krona","ISK","ISL","352","IS","2629691"),
("91","India","India","Asia","Southern Asia","New Delhi","AS",".in","en-IN,hi,bn,te,mr,ta,ur,gu,kn,ml,or,pa,as,bh,sat,ks,ne,sd,kok,doi,mni,sit,sa,fr,lus,inc","Indian Rupee","INR","IND","356","IN","1269750"),
("62","Indonesia","Indonesia","Asia","South-eastern Asia","Jakarta","AS",".id","id,en,nl,jv","Rupiah","IDR","IDN","360","ID","1643084"),
("98","Iran","Iran (Islamic Republic of)","Asia","Southern Asia","Tehran","AS",".ir","fa-IR,ku","Iranian Rial","IRR","IRN","364","IR","130758"),
("964","Iraq","Iraq","Asia","Western Asia","Baghdad","AS",".iq","ar-IQ,ku,hy","Iraqi Dinar","IQD","IRQ","368","IQ","99237"),
("353","Ireland","Ireland","Europe","Northern Europe","Dublin","EU",".ie","en-IE,ga-IE","Euro","EUR","IRL","372","IE","2963597"),
("44","Isle of Man","Isle of Man","Europe","Northern Europe","Douglas","EU",".im","en,gv","Pound Sterling","GBP","IMN","833","IM","3042225"),
("972","Israel","Israel","Asia","Western Asia","Jerusalem","AS",".il","he,ar-IL,en-IL,","New Israeli Sheqel","ILS","ISR","376","IL","294640"),
("39","Italy","Italy","Europe","Southern Europe","Rome","EU",".it","it-IT,de-IT,fr-IT,sc,ca,co,sl","Euro","EUR","ITA","380","IT","3175395"),
("1-876","Jamaica","Jamaica","Americas","Latin America and the Caribbean","Kingston","NA",".jm","en-JM","Jamaican Dollar","JMD","JAM","388","JM","3489940"),
("81","Japan","Japan","Asia","Eastern Asia","Tokyo","AS",".jp","ja","Yen","JPY","JPN","392","JP","1861060"),
("44","Jersey","Jersey","Europe","Northern Europe","Saint Helier","EU",".je","en,fr,nrf","Pound Sterling","GBP","JEY","832","JE","3042142"),
("962","Jordan","Jordan","Asia","Western Asia","Amman","AS",".jo","ar-JO,en","Jordanian Dinar","JOD","JOR","400","JO","248816"),
("7","Kazakhstan","Kazakhstan","Asia","Central Asia","Astana","AS",".kz","kk,ru","Tenge","KZT","KAZ","398","KZ","1522867"),
("254","Kenya","Kenya","Africa","Sub-Saharan Africa","Nairobi","AF",".ke","en-KE,sw-KE","Kenyan Shilling","KES","KEN","404","KE","192950"),
("686","Kiribati","Kiribati","Oceania","Micronesia","Tarawa","OC",".ki","en-KI,gil","Australian Dollar","AUD","KIR","296","KI","4030945"),
("965","Kuwait","Kuwait","Asia","Western Asia","Kuwait City","AS",".kw","ar-KW,en","Kuwaiti Dinar","KWD","KWT","414","KW","285570"),
("996","Kyrgyzstan","Kyrgyzstan","Asia","Central Asia","Bishkek","AS",".kg","ky,uz,ru","Som","KGS","KGZ","417","KG","1527747"),
("856","Laos","Lao People's Democratic Republic","Asia","South-eastern Asia","Vientiane","AS",".la","lo,fr,en","Lao Kip","LAK","LAO","418","LA","1655842"),
("371","Latvia","Latvia","Europe","Northern Europe","Riga","EU",".lv","lv,ru,lt","Euro","EUR","LVA","428","LV","458258"),
("961","Lebanon","Lebanon","Asia","Western Asia","Beirut","AS",".lb","ar-LB,fr-LB,en,hy","Lebanese Pound","LBP","LBN","422","LB","272103"),
("266","Lesotho","Lesotho","Africa","Sub-Saharan Africa","Maseru","AF",".ls","en-LS,st,zu,xh","Loti,Rand","LSL,ZAR","LSO","426","LS","932692"),
("231","Liberia","Liberia","Africa","Sub-Saharan Africa","Monrovia","AF",".lr","en-LR","Liberian Dollar","LRD","LBR","430","LR","2275384"),
("218","Libya","Libya","Africa","Northern Africa","Tripoli","AF",".ly","ar-LY,it,en","Libyan Dinar","LYD","LBY","434","LY","2215636"),
("423","Liechtenstein","Liechtenstein","Europe","Western Europe","Vaduz","EU",".li","de-LI","Swiss Franc","CHF","LIE","438","LI","3042058"),
("370","Lithuania","Lithuania","Europe","Northern Europe","Vilnius","EU",".lt","lt,ru,pl","Euro","EUR","LTU","440","LT","597427"),
("352","Luxembourg","Luxembourg","Europe","Western Europe","Luxembourg","EU",".lu","lb,de-LU,fr-LU","Euro","EUR","LUX","442","LU","2960313"),
("261","Madagascar","Madagascar","Africa","Sub-Saharan Africa","Antananarivo","AF",".mg","fr-MG,mg","Malagasy Ariary","MGA","MDG","450","MG","1062947"),
("265","Malawi","Malawi","Africa","Sub-Saharan Africa","Lilongwe","AF",".mw","ny,yao,tum,swk","Malawi Kwacha","MWK","MWI","454","MW","927384"),
("60","Malaysia","Malaysia","Asia","South-eastern Asia","Kuala Lumpur","AS",".my","ms-MY,en,zh,ta,te,ml,pa,th","Malaysian Ringgit","MYR","MYS","458","MY","1733045"),
("960","Maldives","Maldives","Asia","Southern Asia","Male","AS",".mv","dv,en","Rufiyaa","MVR","MDV","462","MV","1282028"),
("223","Mali","Mali","Africa","Sub-Saharan Africa","Bamako","AF",".ml","fr-ML,bm","CFA Franc BCEAO","XOF","MLI","466","ML","2453866"),
("356","Malta","Malta","Europe","Southern Europe","Valletta","EU",".mt","mt,en-MT","Euro","EUR","MLT","470","MT","2562770"),
("692","Marshall Islands","Marshall Islands","Oceania","Micronesia","Majuro","OC",".mh","mh,en-MH","US Dollar","USD","MHL","584","MH","2080185"),
("596","Martinique","Martinique","Americas","Latin America and the Caribbean","Fort-de-France","NA",".mq","fr-MQ","Euro","EUR","MTQ","474","MQ","3570311"),
("222","Mauritania","Mauritania","Africa","Sub-Saharan Africa","Nouakchott","AF",".mr","ar-MR,fuc,snk,fr,mey,wo","Ouguiya","MRU","MRT","478","MR","2378080"),
("230","Mauritius","Mauritius","Africa","Sub-Saharan Africa","Port Louis","AF",".mu","en-MU,bho,fr","Mauritius Rupee","MUR","MUS","480","MU","934292"),
("262","Mayotte","Mayotte","Africa","Sub-Saharan Africa","Mamoudzou","AF",".yt","fr-YT","Euro","EUR","MYT","175","YT","1024031"),
("52","Mexico","Mexico","Americas","Latin America and the Caribbean","Mexico City","NA",".mx","es-MX","Mexican Peso","MXN","MEX","484","MX","3996063"),
("691","Micronesia","Micronesia (Federated States of)","Oceania","Micronesia","Palikir","OC",".fm","en-FM,chk,pon,yap,kos,uli,woe,nkr,kpg","US Dollar","USD","FSM","583","FM","2081918"),
("377","Monaco","Monaco","Europe","Western Europe","Monaco","EU",".mc","fr-MC,en,it","Euro","EUR","MCO","492","MC","2993457"),
("976","Mongolia","Mongolia","Asia","Eastern Asia","Ulan Bator","AS",".mn","mn,ru","Tugrik","MNT","MNG","496","MN","2029969"),
("382","Montenegro","Montenegro","Europe","Southern Europe","Podgorica","EU",".me","sr,hu,bs,sq,hr,rom","Euro","EUR","MNE","499","ME","3194884"),
("1-664","Montserrat","Montserrat","Americas","Latin America and the Caribbean","Plymouth","NA",".ms","en-MS","East Caribbean Dollar","XCD","MSR","500","MS","3578097"),
("212","Morocco","Morocco","Africa","Northern Africa","Rabat","AF",".ma","ar-MA,ber,fr","Moroccan Dirham","MAD","MAR","504","MA","2542007"),
("258","Mozambique","Mozambique","Africa","Sub-Saharan Africa","Maputo","AF",".mz","pt-MZ,vmw","Mozambique Metical","MZN","MOZ","508","MZ","1036973"),
("95","Myanmar","Myanmar","Asia","South-eastern Asia","Nay Pyi Taw","AS",".mm","my","Kyat","MMK","MMR","104","MM","1327865"),
("264","Namibia","Namibia","Africa","Sub-Saharan Africa","Windhoek","AF",".na","en-NA,af,de,hz,naq","Namibia Dollar,Rand","NAD,ZAR","NAM","516","NA","3355338"),
("674","Nauru","Nauru","Oceania","Micronesia","Yaren","OC",".nr","na,en-NR","Australian Dollar","AUD","NRU","520","NR","2110425"),
("977","Nepal","Nepal","Asia","Southern Asia","Kathmandu","AS",".np","ne,en","Nepalese Rupee","NPR","NPL","524","NP","1282988"),
("31","Netherlands","Netherlands","Europe","Western Europe","Amsterdam","EU",".nl","nl-NL,fy-NL","Euro","EUR","NLD","528","NL","2750405"),
("687","New Caledonia","New Caledonia","Oceania","Melanesia","Noumea","OC",".nc","fr-NC","CFP Franc","XPF","NCL","540","NC","2139685"),
("64","New Zealand","New Zealand","Oceania","Australia and New Zealand","Wellington","OC",".nz","en-NZ,mi","New Zealand Dollar","NZD","NZL","554","NZ","2186224"),
("505","Nicaragua","Nicaragua","Americas","Latin America and the Caribbean","Managua","NA",".ni","es-NI,en","Cordoba Oro","NIO","NIC","558","NI","3617476"),
("227","Niger","Niger","Africa","Sub-Saharan Africa","Niamey","AF",".ne","fr-NE,ha,kr,dje","CFA Franc BCEAO","XOF","NER","562","NE","2440476"),
("234","Nigeria","Nigeria","Africa","Sub-Saharan Africa","Abuja","AF",".ng","en-NG,ha,yo,ig,ff","Naira","NGN","NGA","566","NG","2328926"),
("683","Niue","Niue","Oceania","Polynesia","Alofi","OC",".nu","niu,en-NU","New Zealand Dollar","NZD","NIU","570","NU","4036232"),
("672","Norfolk Island","Norfolk Island","Oceania","Australia and New Zealand","Kingston","OC",".nf","en-NF","Australian Dollar","AUD","NFK","574","NF","2155115"),
("1-670","Northern Mariana Islands","Northern Mariana Islands","Oceania","Micronesia","Saipan","OC",".mp","fil,tl,zh,ch-MP,en-MP","US Dollar","USD","MNP","580","MP","4041468"),
("47","Norway","Norway","Europe","Northern Europe","Oslo","EU",".no","no,nb,nn,se,fi","Norwegian Krone","NOK","NOR","578","NO","3144096"),
("968","Oman","Oman","Asia","Western Asia","Muscat","AS",".om","ar-OM,en,bal,ur","Rial Omani","OMR","OMN","512","OM","286963"),
("92","Pakistan","Pakistan","Asia","Southern Asia","Islamabad","AS",".pk","ur-PK,en-PK,pa,sd,ps,brh","Pakistan Rupee","PKR","PAK","586","PK","1168579"),
("680","Palau","Palau","Oceania","Micronesia","Melekeok","OC",".pw","pau,sov,en-PW,tox,ja,fil,zh","US Dollar","USD","PLW","585","PW","1559582"),
("507","Panama","Panama","Americas","Latin America and the Caribbean","Panama City","NA",".pa","es-PA,en","Balboa,US Dollar","PAB,USD","PAN","591","PA","3703430"),
("675","Papua New Guinea","Papua New Guinea","Oceania","Melanesia","Port Moresby","OC",".pg","en-PG,ho,meu,tpi","Kina","PGK","PNG","598","PG","2088628"),
("595","Paraguay","Paraguay","Americas","Latin America and the Caribbean","Asuncion","SA",".py","es-PY,gn","Guarani","PYG","PRY","600","PY","3437598"),
("51","Peru","Peru","Americas","Latin America and the Caribbean","Lima","SA",".pe","es-PE,qu,ay","Sol","PEN","PER","604","PE","3932488"),
("63","Philippines","Philippines","Asia","South-eastern Asia","Manila","AS",".ph","tl,en-PH,fil,ceb,tgl,ilo,hil,war,pam,bik,bcl,pag,mrw,tsg,mdh,cbk,krj,sgd,msb,akl,ibg,yka,mta,abx","Philippine Peso","PHP","PHL","608","PH","1694008"),
("870","Pitcairn Islands","Pitcairn","Oceania","Polynesia","Adamstown","OC",".pn","en-PN","New Zealand Dollar","NZD","PCN","612","PN","4030699"),
("48","Poland","Poland","Europe","Eastern Europe","Warsaw","EU",".pl","pl","Zloty","PLN","POL","616","PL","798544"),
("351","Portugal","Portugal","Europe","Southern Europe","Lisbon","EU",".pt","pt-PT,mwl","Euro","EUR","PRT","620","PT","2264397"),
("1","Puerto Rico","Puerto Rico","Americas","Latin America and the Caribbean","San Juan","NA",".pr","en-PR,es-PR","US Dollar","USD","PRI","630","PR","4566966"),
("974","Qatar","Qatar","Asia","Western Asia","Doha","AS",".qa","ar-QA,es","Qatari Rial","QAR","QAT","634","QA","289688"),
("82","South Korea","Republic of Korea","Asia","Eastern Asia","Seoul","AS",".kr","ko-KR,en","Won","KRW","KOR","410","KR","1835841"),
("373","Moldova","Republic of Moldova","Europe","Eastern Europe","Chisinau","EU",".md","ro,ru,gag,tr","Moldovan Leu","MDL","MDA","498","MD","617790"),
("40","Romania","Romania","Europe","Eastern Europe","Bucharest","EU",".ro","ro,hu,rom","Romanian Leu","RON","ROU","642","RO","798549"),
("7","Russia","Russian Federation","Europe","Eastern Europe","Moscow","EU",".ru","ru,tt,xal,cau,ady,kv,ce,tyv,cv,udm,tut,mns,bua,myv,mdf,chm,ba,inh,tut,kbd,krc,av,sah,nog","Russian Ruble","RUB","RUS","643","RU","2017370"),
("250","Rwanda","Rwanda","Africa","Sub-Saharan Africa","Kigali","AF",".rw","rw,en-RW,fr-RW,sw","Rwanda Franc","RWF","RWA","646","RW","49518"),
("262","RÃ©union","RÃ©union","Africa","Sub-Saharan Africa","Saint-Denis","AF",".re","fr-RE","Euro","EUR","REU","638","RE","935317"),
("590","St. BarthÃ©lemy","Saint BarthÃ©lemy","Americas","Latin America and the Caribbean","Gustavia","NA",".gp","fr","Euro","EUR","BLM","652","BL","3578476"),
("290","St. Helena","Saint Helena","Africa","Sub-Saharan Africa","Jamestown","AF",".sh","en-SH","Saint Helena Pound","SHP","SHN","654","SH","3370751"),
("1-869","St. Kitts & Nevis","Saint Kitts and Nevis","Americas","Latin America and the Caribbean","Basseterre","NA",".kn","en-KN","East Caribbean Dollar","XCD","KNA","659","KN","3575174"),
("1-758","St. Lucia","Saint Lucia","Americas","Latin America and the Caribbean","Castries","NA",".lc","en-LC","East Caribbean Dollar","XCD","LCA","662","LC","3576468"),
("590","St. Martin","Saint Martin (French Part)","Americas","Latin America and the Caribbean","Marigot","NA",".gp","fr","Euro","EUR","MAF","663","MF","3578421"),
("508","St. Pierre & Miquelon","Saint Pierre and Miquelon","Americas","Northern America","Saint-Pierre","NA",".pm","fr-PM","Euro","EUR","SPM","666","PM","3424932"),
("1-784","St. Vincent & Grenadines","Saint Vincent and the Grenadines","Americas","Latin America and the Caribbean","Kingstown","NA",".vc","en-VC,fr","East Caribbean Dollar","XCD","VCT","670","VC","3577815"),
("685","Samoa","Samoa","Oceania","Polynesia","Apia","OC",".ws","sm,en-WS","Tala","WST","WSM","882","WS","4034894"),
("378","San Marino","San Marino","Europe","Southern Europe","San Marino","EU",".sm","it-SM","Euro","EUR","SMR","674","SM","3168068"),
("239","SÃ£o TomÃ© & PrÃ­ncipe","Sao Tome and Principe","Africa","Sub-Saharan Africa","Sao Tome","AF",".st","pt-ST","Dobra","STN","STP","678","ST","2410758"),
("","","Sark","Europe","Northern Europe","","","","","","","","","",""),
("966","Saudi Arabia","Saudi Arabia","Asia","Western Asia","Riyadh","AS",".sa","ar-SA","Saudi Riyal","SAR","SAU","682","SA","102358"),
("221","Senegal","Senegal","Africa","Sub-Saharan Africa","Dakar","AF",".sn","fr-SN,wo,fuc,mnk","CFA Franc BCEAO","XOF","SEN","686","SN","2245662"),
("381","Serbia","Serbia","Europe","Southern Europe","Belgrade","EU",".rs","sr,hu,bs,rom","Serbian Dinar","RSD","SRB","688","RS","6290252"),
("248","Seychelles","Seychelles","Africa","Sub-Saharan Africa","Victoria","AF",".sc","en-SC,fr-SC","Seychelles Rupee","SCR","SYC","690","SC","241170"),
("232","Sierra Leone","Sierra Leone","Africa","Sub-Saharan Africa","Freetown","AF",".sl","en-SL,men,tem","Leone","SLL","SLE","694","SL","2403846"),
("65","Singapore","Singapore","Asia","South-eastern Asia","Singapore","AS",".sg","cmn,en-SG,ms-SG,ta-SG,zh-SG","Singapore Dollar","SGD","SGP","702","SG","1880251"),
("1-721","Sint Maarten","Sint Maarten (Dutch part)","Americas","Latin America and the Caribbean","Philipsburg","NA",".sx","nl,en","Netherlands Antillean Guilder","ANG","SXM","534","SX","7609695"),
("421","Slovakia","Slovakia","Europe","Eastern Europe","Bratislava","EU",".sk","sk,hu","Euro","EUR","SVK","703","SK","3057568"),
("386","Slovenia","Slovenia","Europe","Southern Europe","Ljubljana","EU",".si","sl,sh","Euro","EUR","SVN","705","SI","3190538"),
("677","Solomon Islands","Solomon Islands","Oceania","Melanesia","Honiara","OC",".sb","en-SB,tpi","Solomon Islands Dollar","SBD","SLB","90","SB","2103350"),
("252","Somalia","Somalia","Africa","Sub-Saharan Africa","Mogadishu","AF",".so","so-SO,ar-SO,it,en-SO","Somali Shilling","SOS","SOM","706","SO","51537"),
("27","South Africa","South Africa","Africa","Sub-Saharan Africa","Pretoria","AF",".za","zu,xh,af,nso,en-ZA,tn,st,ts,ss,ve,nr","Rand","ZAR","ZAF","710","ZA","953987"),
("500","South Georgia & South Sandwich Islands","South Georgia and the South Sandwich Islands","Americas","Latin America and the Caribbean","Grytviken","AN",".gs","en","No universal currency","","SGS","239","GS","3474415"),
("211","South Sudan","South Sudan","Africa","Sub-Saharan Africa","Juba","AF","","en","South Sudanese Pound","SSP","SSD","728","SS","7909807"),
("34","Spain","Spain","Europe","Southern Europe","Madrid","EU",".es","es-ES,ca,gl,eu,oc","Euro","EUR","ESP","724","ES","2510769"),
("94","Sri Lanka","Sri Lanka","Asia","Southern Asia","Colombo","AS",".lk","si,ta,en","Sri Lanka Rupee","LKR","LKA","144","LK","1227603"),
("970","Palestine","State of Palestine","Asia","Western Asia","East Jerusalem","AS",".ps","ar-PS","No universal currency","","PSE","275","PS","6254930"),
("249","Sudan","Sudan","Africa","Northern Africa","Khartoum","AF",".sd","ar-SD,en,fia","Sudanese Pound","SDG","SDN","729","SD","366755"),
("597","Suriname","Suriname","Americas","Latin America and the Caribbean","Paramaribo","SA",".sr","nl-SR,en,srn,hns,jv","Surinam Dollar","SRD","SUR","740","SR","3382998"),
("47","Svalbard & Jan Mayen","Svalbard and Jan Mayen Islands","Europe","Northern Europe","Longyearbyen","EU",".sj","no,ru","Norwegian Krone","NOK","SJM","744","SJ","607072"),
("46","Sweden","Sweden","Europe","Northern Europe","Stockholm","EU",".se","sv-SE,se,sma,fi-SE","Swedish Krona","SEK","SWE","752","SE","2661886"),
("41","Switzerland","Switzerland","Europe","Western Europe","Bern","EU",".ch","de-CH,fr-CH,it-CH,rm","Swiss Franc","CHF","CHE","756","CH","2658434"),
("963","Syria","Syrian Arab Republic","Asia","Western Asia","Damascus","AS",".sy","ar-SY,ku,hy,arc,fr,en","Syrian Pound","SYP","SYR","760","SY","163843"),
("992","Tajikistan","Tajikistan","Asia","Central Asia","Dushanbe","AS",".tj","tg,ru","Somoni","TJS","TJK","762","TJ","1220409"),
("66","Thailand","Thailand","Asia","South-eastern Asia","Bangkok","AS",".th","th,en","Baht","THB","THA","764","TH","1605651"),
("389","North Macedonia","The former Yugoslav Republic of Macedonia","Europe","Southern Europe","Skopje","EU",".mk","mk,sq,tr,rmm,sr","Denar","MKD","MKD","807","MK","718075"),
("670","Timor-Leste","Timor-Leste","Asia","South-eastern Asia","Dili","OC",".tl","tet,pt-TL,id,en","US Dollar","USD","TLS","626","TL","1966436"),
("228","Togo","Togo","Africa","Sub-Saharan Africa","Lome","AF",".tg","fr-TG,ee,hna,kbp,dag,ha","CFA Franc BCEAO","XOF","TGO","768","TG","2363686"),
("690","Tokelau","Tokelau","Oceania","Polynesia","","OC",".tk","tkl,en-TK","New Zealand Dollar","NZD","TKL","772","TK","4031074"),
("676","Tonga","Tonga","Oceania","Polynesia","Nuku'alofa","OC",".to","to,en-TO","Paâ€™anga","TOP","TON","776","TO","4032283"),
("1-868","Trinidad & Tobago","Trinidad and Tobago","Americas","Latin America and the Caribbean","Port of Spain","NA",".tt","en-TT,hns,fr,es,zh","Trinidad and Tobago Dollar","TTD","TTO","780","TT","3573591"),
("216","Tunisia","Tunisia","Africa","Northern Africa","Tunis","AF",".tn","ar-TN,fr","Tunisian Dinar","TND","TUN","788","TN","2464461"),
("90","Turkey","Turkey","Asia","Western Asia","Ankara","AS",".tr","tr-TR,ku,diq,az,av","Turkish Lira","TRY","TUR","792","TR","298795"),
("993","Turkmenistan","Turkmenistan","Asia","Central Asia","Ashgabat","AS",".tm","tk,ru,uz","Turkmenistan New Manat","TMT","TKM","795","TM","1218197"),
("1-649","Turks & Caicos Islands","Turks and Caicos Islands","Americas","Latin America and the Caribbean","Cockburn Town","NA",".tc","en-TC","US Dollar","USD","TCA","796","TC","3576916"),
("688","Tuvalu","Tuvalu","Oceania","Polynesia","Funafuti","OC",".tv","tvl,en,sm,gil","Australian Dollar","AUD","TUV","798","TV","2110297"),
("256","Uganda","Uganda","Africa","Sub-Saharan Africa","Kampala","AF",".ug","en-UG,lg,sw,ar","Uganda Shilling","UGX","UGA","800","UG","226074"),
("380","Ukraine","Ukraine","Europe","Eastern Europe","Kyiv","EU",".ua","uk,ru-UA,rom,pl,hu","Hryvnia","UAH","UKR","804","UA","690791"),
("971","United Arab Emirates","United Arab Emirates","Asia","Western Asia","Abu Dhabi","AS",".ae","ar-AE,fa,en,hi,ur","UAE Dirham","AED","ARE","784","AE","290557"),
("44","UK","United Kingdom of Great Britain and Northern Ireland","Europe","Northern Europe","London","EU",".uk","en-GB,cy-GB,gd","Pound Sterling","GBP","GBR","826","GB","2635167"),
("255","Tanzania","United Republic of Tanzania","Africa","Sub-Saharan Africa","Dodoma","AF",".tz","sw-TZ,en,ar","Tanzanian Shilling","TZS","TZA","834","TZ","149590"),
("Â ","U.S. Outlying Islands","United States Minor Outlying Islands","Oceania","Micronesia","","OC",".um","en-UM","US Dollar","USD","UMI","581","UM","5854968"),
("1-340","U.S. Virgin Islands","United States Virgin Islands","Americas","Latin America and the Caribbean","Charlotte Amalie","NA",".vi","en-VI","US Dollar","USD","VIR","850","VI","4796775"),
("1","US","United States of America","Americas","Northern America","Washington","NA",".us","en-US,es-US,haw,fr","US Dollar","USD","USA","840","US","6252001"),
("598","Uruguay","Uruguay","Americas","Latin America and the Caribbean","Montevideo","SA",".uy","es-UY","Peso Uruguayo","UYU","URY","858","UY","3439705"),
("998","Uzbekistan","Uzbekistan","Asia","Central Asia","Tashkent","AS",".uz","uz,ru,tg","Uzbekistan Sum","UZS","UZB","860","UZ","1512440"),
("678","Vanuatu","Vanuatu","Oceania","Melanesia","Port Vila","OC",".vu","bi,en-VU,fr-VU","Vatu","VUV","VUT","548","VU","2134431"),
("58","Venezuela","Venezuela (Bolivarian Republic of)","Americas","Latin America and the Caribbean","Caracas","SA",".ve","es-VE","BolÃ­var","VES","VEN","862","VE","3625428"),
("84","Vietnam","Viet Nam","Asia","South-eastern Asia","Hanoi","AS",".vn","vi,en,fr,zh,km","Dong","VND","VNM","704","VN","1562822"),
("681","Wallis & Futuna","Wallis and Futuna Islands","Oceania","Polynesia","Mata Utu","OC",".wf","wls,fud,fr-WF","CFP Franc","XPF","WLF","876","WF","4034749"),
("212","Western Sahara","Western Sahara","Africa","Northern Africa","El-Aaiun","AF",".eh","ar,mey","Moroccan Dirham","MAD","ESH","732","EH","2461445"),
("967","Yemen","Yemen","Asia","Western Asia","Sanaa","AS",".ye","ar-YE","Yemeni Rial","YER","YEM","887","YE","69543"),
("260","Zambia","Zambia","Africa","Sub-Saharan Africa","Lusaka","AF",".zm","en-ZM,bem,loz,lun,lue,ny,toi","Zambian Kwacha","ZMW","ZMB","894","ZM","895949"),
("263","Zimbabwe","Zimbabwe","Africa","Sub-Saharan Africa","Harare","AF",".zw","en-ZW,sn,nr,nd","Zimbabwe Dollar","ZWL","ZWE","716","ZW","878675"),
("358","Ã…land Islands","Ã…land Islands","Europe","Northern Europe","Mariehamn","EU",".ax","sv-AX","Euro","EUR","ALA","248","AX","661882"),

        };
    }
}
