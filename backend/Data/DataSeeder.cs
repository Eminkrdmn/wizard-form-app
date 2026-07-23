using WizardFormApi.Models;

namespace WizardFormApi.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Departments.Any())
            return;

        var departments = CreateDepartments();
        context.Departments.AddRange(departments);
        context.SaveChanges();

        var roles = CreateRoles(departments);
        context.Roles.AddRange(roles);
        context.SaveChanges();

        var users = CreateUsers(roles, departments);
        context.Users.AddRange(users);
        context.SaveChanges();
    }

    private static List<Department> CreateDepartments()
    {
        return new List<Department>
        {
            new() { Name = "Üst Yönetim", Code = "UST_YONETIM" },
            new() { Name = "Sera Operasyonları", Code = "SERA" },
            new() { Name = "Lojistik & Depo", Code = "LOJISTIK" },
            new() { Name = "Mağaza Operasyonları", Code = "MAGAZA" },
            new() { Name = "Satın Alma", Code = "SATIN_ALMA" },
            new() { Name = "İnsan Kaynakları", Code = "IK" },
            new() { Name = "Finans & Muhasebe", Code = "FINANS" },
            new() { Name = "Kalite Kontrol", Code = "KALITE" },
            new() { Name = "Bilgi Teknolojileri", Code = "IT" },
            new() { Name = "Pazarlama", Code = "PAZARLAMA" },
            new() { Name = "Franchise", Code = "FRANCHISE" }
        };
    }

    private static List<Role> CreateRoles(List<Department> depts)
    {
        var d = depts.ToDictionary(x => x.Code);

        return new List<Role>
        {
            // Üst Yönetim (Level 1)
            new() { Name = "Genel Müdür", Code = "GENEL_MUDUR", Department = d["UST_YONETIM"], Level = 1 },
            new() { Name = "Operasyon Direktörü", Code = "OPERASYON_DIREKTORU", Department = d["UST_YONETIM"], Level = 1 },
            new() { Name = "Hukuk Danışmanı", Code = "HUKUK_DANISMANI", Department = d["UST_YONETIM"], Level = 3 },

            // Sera Operasyonları
            new() { Name = "Sera Operasyonları Müdürü", Code = "SERA_MUDURU", Department = d["SERA"], Level = 2 },
            new() { Name = "Sera Şefi", Code = "SERA_SEFI", Department = d["SERA"], Level = 3 },
            new() { Name = "Florist", Code = "FLORIST", Department = d["SERA"], Level = 4 },
            new() { Name = "Sera İşçisi", Code = "SERA_ISCISI", Department = d["SERA"], Level = 4 },

            // Lojistik & Depo
            new() { Name = "Lojistik Müdürü", Code = "LOJISTIK_MUDURU", Department = d["LOJISTIK"], Level = 2 },
            new() { Name = "Depo Sorumlusu", Code = "DEPO_SORUMLUSU", Department = d["LOJISTIK"], Level = 3 },
            new() { Name = "Şoför", Code = "SOFOR", Department = d["LOJISTIK"], Level = 4 },
            new() { Name = "Kurye", Code = "KURYE", Department = d["LOJISTIK"], Level = 4 },

            // Mağaza Operasyonları
            new() { Name = "Bölge Müdürü", Code = "BOLGE_MUDURU", Department = d["MAGAZA"], Level = 2 },
            new() { Name = "Mağaza Müdürü", Code = "MAGAZA_MUDURU", Department = d["MAGAZA"], Level = 3 },
            new() { Name = "Satış Danışmanı", Code = "SATIS_DANISMANI", Department = d["MAGAZA"], Level = 4 },
            new() { Name = "Kasiyer", Code = "KASIYER", Department = d["MAGAZA"], Level = 4 },

            // Satın Alma
            new() { Name = "Satın Alma Müdürü", Code = "SATIN_ALMA_MUDURU", Department = d["SATIN_ALMA"], Level = 2 },
            new() { Name = "Satın Alma Uzmanı", Code = "SATIN_ALMA_UZMANI", Department = d["SATIN_ALMA"], Level = 3 },

            // İnsan Kaynakları
            new() { Name = "İK Müdürü", Code = "IK_MUDURU", Department = d["IK"], Level = 2 },
            new() { Name = "İK Uzmanı", Code = "IK_UZMANI", Department = d["IK"], Level = 3 },
            new() { Name = "İdari İşler Sorumlusu", Code = "IDARI_ISLER_SORUMLUSU", Department = d["IK"], Level = 3 },

            // Finans & Muhasebe
            new() { Name = "Finans Müdürü", Code = "FINANS_MUDURU", Department = d["FINANS"], Level = 2 },
            new() { Name = "Muhasebe Uzmanı", Code = "MUHASEBE_UZMANI", Department = d["FINANS"], Level = 3 },

            // Kalite Kontrol
            new() { Name = "Kalite Kontrol Müdürü", Code = "KALITE_MUDURU", Department = d["KALITE"], Level = 2 },
            new() { Name = "Kalite Kontrol Uzmanı", Code = "KALITE_UZMANI", Department = d["KALITE"], Level = 3 },

            // Bilgi Teknolojileri
            new() { Name = "IT Müdürü", Code = "IT_MUDURU", Department = d["IT"], Level = 2 },
            new() { Name = "Sistem Yöneticisi", Code = "SISTEM_YONETICISI", Department = d["IT"], Level = 3 },

            // Pazarlama
            new() { Name = "Pazarlama Müdürü", Code = "PAZARLAMA_MUDURU", Department = d["PAZARLAMA"], Level = 2 },
            new() { Name = "Dijital Pazarlama Uzmanı", Code = "DIJITAL_PAZARLAMA_UZMANI", Department = d["PAZARLAMA"], Level = 3 },

            // Franchise
            new() { Name = "Franchise Müdürü", Code = "FRANCHISE_MUDURU", Department = d["FRANCHISE"], Level = 2 },
            new() { Name = "Franchise Danışmanı", Code = "FRANCHISE_DANISMANI", Department = d["FRANCHISE"], Level = 3 },
        };
    }

    private static List<User> CreateUsers(List<Role> roles, List<Department> departments)
    {
        var r = roles.ToDictionary(x => x.Code);
        var defaultPassword = BCrypt.Net.BCrypt.HashPassword("123456");

        return new List<User>
        {
            // Üst Yönetim
            new() { Username = "ahmet.yilmaz", DisplayName = "Ahmet Yılmaz", Email = "ahmet.yilmaz@sinemincicek.com", PasswordHash = defaultPassword, Role = r["GENEL_MUDUR"], Department = r["GENEL_MUDUR"].Department },
            new() { Username = "ayse.demir", DisplayName = "Ayşe Demir", Email = "ayse.demir@sinemincicek.com", PasswordHash = defaultPassword, Role = r["OPERASYON_DIREKTORU"], Department = r["OPERASYON_DIREKTORU"].Department },
            new() { Username = "mehmet.kara", DisplayName = "Mehmet Kara", Email = "mehmet.kara@sinemincicek.com", PasswordHash = defaultPassword, Role = r["HUKUK_DANISMANI"], Department = r["HUKUK_DANISMANI"].Department },

            // Sera Operasyonları
            new() { Username = "fatma.ozturk", DisplayName = "Fatma Öztürk", Email = "fatma.ozturk@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SERA_MUDURU"], Department = r["SERA_MUDURU"].Department },
            new() { Username = "ali.celik", DisplayName = "Ali Çelik", Email = "ali.celik@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SERA_SEFI"], Department = r["SERA_SEFI"].Department },
            new() { Username = "zeynep.arslan", DisplayName = "Zeynep Arslan", Email = "zeynep.arslan@sinemincicek.com", PasswordHash = defaultPassword, Role = r["FLORIST"], Department = r["FLORIST"].Department },
            new() { Username = "mustafa.sahin", DisplayName = "Mustafa Şahin", Email = "mustafa.sahin@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SERA_ISCISI"], Department = r["SERA_ISCISI"].Department },

            // Lojistik & Depo
            new() { Username = "emine.koç", DisplayName = "Emine Koç", Email = "emine.koc@sinemincicek.com", PasswordHash = defaultPassword, Role = r["LOJISTIK_MUDURU"], Department = r["LOJISTIK_MUDURU"].Department },
            new() { Username = "hasan.yildiz", DisplayName = "Hasan Yıldız", Email = "hasan.yildiz@sinemincicek.com", PasswordHash = defaultPassword, Role = r["DEPO_SORUMLUSU"], Department = r["DEPO_SORUMLUSU"].Department },
            new() { Username = "huseyin.aydin", DisplayName = "Hüseyin Aydın", Email = "huseyin.aydin@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SOFOR"], Department = r["SOFOR"].Department },
            new() { Username = "ibrahim.polat", DisplayName = "İbrahim Polat", Email = "ibrahim.polat@sinemincicek.com", PasswordHash = defaultPassword, Role = r["KURYE"], Department = r["KURYE"].Department },

            // Mağaza Operasyonları
            new() { Username = "hacer.erdogan", DisplayName = "Hacer Erdoğan", Email = "hacer.erdogan@sinemincicek.com", PasswordHash = defaultPassword, Role = r["BOLGE_MUDURU"], Department = r["BOLGE_MUDURU"].Department },
            new() { Username = "omer.tas", DisplayName = "Ömer Taş", Email = "omer.tas@sinemincicek.com", PasswordHash = defaultPassword, Role = r["MAGAZA_MUDURU"], Department = r["MAGAZA_MUDURU"].Department },
            new() { Username = "elif.kilic", DisplayName = "Elif Kılıç", Email = "elif.kilic@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SATIS_DANISMANI"], Department = r["SATIS_DANISMANI"].Department },
            new() { Username = "burak.sen", DisplayName = "Burak Şen", Email = "burak.sen@sinemincicek.com", PasswordHash = defaultPassword, Role = r["KASIYER"], Department = r["KASIYER"].Department },

            // Satın Alma
            new() { Username = "sevgi.dogan", DisplayName = "Sevgi Doğan", Email = "sevgi.dogan@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SATIN_ALMA_MUDURU"], Department = r["SATIN_ALMA_MUDURU"].Department },
            new() { Username = "kemal.aksoy", DisplayName = "Kemal Aksoy", Email = "kemal.aksoy@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SATIN_ALMA_UZMANI"], Department = r["SATIN_ALMA_UZMANI"].Department },

            // İnsan Kaynakları
            new() { Username = "derya.gunes", DisplayName = "Derya Güneş", Email = "derya.gunes@sinemincicek.com", PasswordHash = defaultPassword, Role = r["IK_MUDURU"], Department = r["IK_MUDURU"].Department },
            new() { Username = "tolga.kurt", DisplayName = "Tolga Kurt", Email = "tolga.kurt@sinemincicek.com", PasswordHash = defaultPassword, Role = r["IK_UZMANI"], Department = r["IK_UZMANI"].Department },
            new() { Username = "gul.bay", DisplayName = "Gül Bay", Email = "gul.bay@sinemincicek.com", PasswordHash = defaultPassword, Role = r["IDARI_ISLER_SORUMLUSU"], Department = r["IDARI_ISLER_SORUMLUSU"].Department },

            // Finans & Muhasebe
            new() { Username = "serkan.ozdemir", DisplayName = "Serkan Özdemir", Email = "serkan.ozdemir@sinemincicek.com", PasswordHash = defaultPassword, Role = r["FINANS_MUDURU"], Department = r["FINANS_MUDURU"].Department },
            new() { Username = "neslihan.korkmaz", DisplayName = "Neslihan Korkmaz", Email = "neslihan.korkmaz@sinemincicek.com", PasswordHash = defaultPassword, Role = r["MUHASEBE_UZMANI"], Department = r["MUHASEBE_UZMANI"].Department },

            // Kalite Kontrol
            new() { Username = "baris.ozkan", DisplayName = "Barış Özkan", Email = "baris.ozkan@sinemincicek.com", PasswordHash = defaultPassword, Role = r["KALITE_MUDURU"], Department = r["KALITE_MUDURU"].Department },
            new() { Username = "selin.tekin", DisplayName = "Selin Tekin", Email = "selin.tekin@sinemincicek.com", PasswordHash = defaultPassword, Role = r["KALITE_UZMANI"], Department = r["KALITE_UZMANI"].Department },

            // Bilgi Teknolojileri
            new() { Username = "can.yildirim", DisplayName = "Can Yıldırım", Email = "can.yildirim@sinemincicek.com", PasswordHash = defaultPassword, Role = r["IT_MUDURU"], Department = r["IT_MUDURU"].Department },
            new() { Username = "emre.karaca", DisplayName = "Emre Karaca", Email = "emre.karaca@sinemincicek.com", PasswordHash = defaultPassword, Role = r["SISTEM_YONETICISI"], Department = r["SISTEM_YONETICISI"].Department },

            // Pazarlama
            new() { Username = "pinar.erdem", DisplayName = "Pınar Erdem", Email = "pinar.erdem@sinemincicek.com", PasswordHash = defaultPassword, Role = r["PAZARLAMA_MUDURU"], Department = r["PAZARLAMA_MUDURU"].Department },
            new() { Username = "deniz.cinar", DisplayName = "Deniz Çınar", Email = "deniz.cinar@sinemincicek.com", PasswordHash = defaultPassword, Role = r["DIJITAL_PAZARLAMA_UZMANI"], Department = r["DIJITAL_PAZARLAMA_UZMANI"].Department },

            // Franchise
            new() { Username = "murat.aslan", DisplayName = "Murat Aslan", Email = "murat.aslan@sinemincicek.com", PasswordHash = defaultPassword, Role = r["FRANCHISE_MUDURU"], Department = r["FRANCHISE_MUDURU"].Department },
            new() { Username = "sibel.uysal", DisplayName = "Sibel Uysal", Email = "sibel.uysal@sinemincicek.com", PasswordHash = defaultPassword, Role = r["FRANCHISE_DANISMANI"], Department = r["FRANCHISE_DANISMANI"].Department },
        };
    }
}
