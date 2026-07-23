using WizardFormApi.Models;

namespace WizardFormApi.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Departments.Any())
        {
            var departments = CreateDepartments();
            context.Departments.AddRange(departments);
            context.SaveChanges();

            var roles = CreateRoles(departments);
            context.Roles.AddRange(roles);
            context.SaveChanges();

            var users = CreateUsers(roles);
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        if (!context.WorkflowDefinitions.Any())
        {
            var forms = CreateFormTemplates();
            context.Forms.AddRange(forms);
            context.SaveChanges();

            var workflows = CreateWorkflowDefinitions(forms);
            context.WorkflowDefinitions.AddRange(workflows);
            context.SaveChanges();

            var roles = context.Roles.ToDictionary(r => r.Code);
            var steps = CreateWorkflowSteps(workflows, roles);
            context.WorkflowSteps.AddRange(steps);
            context.SaveChanges();
        }
    }

    // ══════════════════════════════════════════
    //  DEPARTMENTS
    // ══════════════════════════════════════════

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

    // ══════════════════════════════════════════
    //  ROLES
    // ══════════════════════════════════════════

    private static List<Role> CreateRoles(List<Department> depts)
    {
        var d = depts.ToDictionary(x => x.Code);

        return new List<Role>
        {
            new() { Name = "Genel Müdür", Code = "GENEL_MUDUR", Department = d["UST_YONETIM"], Level = 1 },
            new() { Name = "Operasyon Direktörü", Code = "OPERASYON_DIREKTORU", Department = d["UST_YONETIM"], Level = 1 },
            new() { Name = "Hukuk Danışmanı", Code = "HUKUK_DANISMANI", Department = d["UST_YONETIM"], Level = 3 },

            new() { Name = "Sera Operasyonları Müdürü", Code = "SERA_MUDURU", Department = d["SERA"], Level = 2 },
            new() { Name = "Sera Şefi", Code = "SERA_SEFI", Department = d["SERA"], Level = 3 },
            new() { Name = "Florist", Code = "FLORIST", Department = d["SERA"], Level = 4 },
            new() { Name = "Sera İşçisi", Code = "SERA_ISCISI", Department = d["SERA"], Level = 4 },

            new() { Name = "Lojistik Müdürü", Code = "LOJISTIK_MUDURU", Department = d["LOJISTIK"], Level = 2 },
            new() { Name = "Depo Sorumlusu", Code = "DEPO_SORUMLUSU", Department = d["LOJISTIK"], Level = 3 },
            new() { Name = "Şoför", Code = "SOFOR", Department = d["LOJISTIK"], Level = 4 },
            new() { Name = "Kurye", Code = "KURYE", Department = d["LOJISTIK"], Level = 4 },

            new() { Name = "Bölge Müdürü", Code = "BOLGE_MUDURU", Department = d["MAGAZA"], Level = 2 },
            new() { Name = "Mağaza Müdürü", Code = "MAGAZA_MUDURU", Department = d["MAGAZA"], Level = 3 },
            new() { Name = "Satış Danışmanı", Code = "SATIS_DANISMANI", Department = d["MAGAZA"], Level = 4 },
            new() { Name = "Kasiyer", Code = "KASIYER", Department = d["MAGAZA"], Level = 4 },

            new() { Name = "Satın Alma Müdürü", Code = "SATIN_ALMA_MUDURU", Department = d["SATIN_ALMA"], Level = 2 },
            new() { Name = "Satın Alma Uzmanı", Code = "SATIN_ALMA_UZMANI", Department = d["SATIN_ALMA"], Level = 3 },

            new() { Name = "İK Müdürü", Code = "IK_MUDURU", Department = d["IK"], Level = 2 },
            new() { Name = "İK Uzmanı", Code = "IK_UZMANI", Department = d["IK"], Level = 3 },
            new() { Name = "İdari İşler Sorumlusu", Code = "IDARI_ISLER_SORUMLUSU", Department = d["IK"], Level = 3 },

            new() { Name = "Finans Müdürü", Code = "FINANS_MUDURU", Department = d["FINANS"], Level = 2 },
            new() { Name = "Muhasebe Uzmanı", Code = "MUHASEBE_UZMANI", Department = d["FINANS"], Level = 3 },

            new() { Name = "Kalite Kontrol Müdürü", Code = "KALITE_MUDURU", Department = d["KALITE"], Level = 2 },
            new() { Name = "Kalite Kontrol Uzmanı", Code = "KALITE_UZMANI", Department = d["KALITE"], Level = 3 },

            new() { Name = "IT Müdürü", Code = "IT_MUDURU", Department = d["IT"], Level = 2 },
            new() { Name = "Sistem Yöneticisi", Code = "SISTEM_YONETICISI", Department = d["IT"], Level = 3 },

            new() { Name = "Pazarlama Müdürü", Code = "PAZARLAMA_MUDURU", Department = d["PAZARLAMA"], Level = 2 },
            new() { Name = "Dijital Pazarlama Uzmanı", Code = "DIJITAL_PAZARLAMA_UZMANI", Department = d["PAZARLAMA"], Level = 3 },

            new() { Name = "Franchise Müdürü", Code = "FRANCHISE_MUDURU", Department = d["FRANCHISE"], Level = 2 },
            new() { Name = "Franchise Danışmanı", Code = "FRANCHISE_DANISMANI", Department = d["FRANCHISE"], Level = 3 },
        };
    }

    // ══════════════════════════════════════════
    //  USERS
    // ══════════════════════════════════════════

    private static List<User> CreateUsers(List<Role> roles)
    {
        var r = roles.ToDictionary(x => x.Code);
        var pw = BCrypt.Net.BCrypt.HashPassword("123456");

        return new List<User>
        {
            U("ahmet.yilmaz", "Ahmet Yılmaz", r["GENEL_MUDUR"], pw),
            U("ayse.demir", "Ayşe Demir", r["OPERASYON_DIREKTORU"], pw),
            U("mehmet.kara", "Mehmet Kara", r["HUKUK_DANISMANI"], pw),

            U("fatma.ozturk", "Fatma Öztürk", r["SERA_MUDURU"], pw),
            U("ali.celik", "Ali Çelik", r["SERA_SEFI"], pw),
            U("zeynep.arslan", "Zeynep Arslan", r["FLORIST"], pw),
            U("mustafa.sahin", "Mustafa Şahin", r["SERA_ISCISI"], pw),

            U("emine.koc", "Emine Koç", r["LOJISTIK_MUDURU"], pw),
            U("hasan.yildiz", "Hasan Yıldız", r["DEPO_SORUMLUSU"], pw),
            U("huseyin.aydin", "Hüseyin Aydın", r["SOFOR"], pw),
            U("ibrahim.polat", "İbrahim Polat", r["KURYE"], pw),

            U("hacer.erdogan", "Hacer Erdoğan", r["BOLGE_MUDURU"], pw),
            U("omer.tas", "Ömer Taş", r["MAGAZA_MUDURU"], pw),
            U("elif.kilic", "Elif Kılıç", r["SATIS_DANISMANI"], pw),
            U("burak.sen", "Burak Şen", r["KASIYER"], pw),

            U("sevgi.dogan", "Sevgi Doğan", r["SATIN_ALMA_MUDURU"], pw),
            U("kemal.aksoy", "Kemal Aksoy", r["SATIN_ALMA_UZMANI"], pw),

            U("derya.gunes", "Derya Güneş", r["IK_MUDURU"], pw),
            U("tolga.kurt", "Tolga Kurt", r["IK_UZMANI"], pw),
            U("gul.bay", "Gül Bay", r["IDARI_ISLER_SORUMLUSU"], pw),

            U("serkan.ozdemir", "Serkan Özdemir", r["FINANS_MUDURU"], pw),
            U("neslihan.korkmaz", "Neslihan Korkmaz", r["MUHASEBE_UZMANI"], pw),

            U("baris.ozkan", "Barış Özkan", r["KALITE_MUDURU"], pw),
            U("selin.tekin", "Selin Tekin", r["KALITE_UZMANI"], pw),

            U("can.yildirim", "Can Yıldırım", r["IT_MUDURU"], pw),
            U("emre.karaca", "Emre Karaca", r["SISTEM_YONETICISI"], pw),

            U("pinar.erdem", "Pınar Erdem", r["PAZARLAMA_MUDURU"], pw),
            U("deniz.cinar", "Deniz Çınar", r["DIJITAL_PAZARLAMA_UZMANI"], pw),

            U("murat.aslan", "Murat Aslan", r["FRANCHISE_MUDURU"], pw),
            U("sibel.uysal", "Sibel Uysal", r["FRANCHISE_DANISMANI"], pw),
        };
    }

    private static User U(string username, string displayName, Role role, string passwordHash) => new()
    {
        Username = username,
        DisplayName = displayName,
        Email = $"{username}@sinemincicek.com",
        PasswordHash = passwordHash,
        Role = role,
        Department = role.Department
    };

    // ══════════════════════════════════════════
    //  FORM TEMPLATES (4 working + 11 placeholder)
    // ══════════════════════════════════════════

    private static List<FormDefinition> CreateFormTemplates()
    {
        return new List<FormDefinition>
        {
            // ── 4 Tam Çalışan Sürecin Formları ──

            new()
            {
                Name = "İzin Talebi Formu",
                Description = "Yıllık, mazeret, hastalık veya doğum izni talep formu",
                IsSystemForm = true,
                FieldsJson = """
                [
                    {"id":"izin_turu","label":"İzin Türü","type":"select","required":true,"options":["Yıllık İzin","Mazeret İzni","Hastalık İzni","Doğum İzni"]},
                    {"id":"baslangic_tarihi","label":"Başlangıç Tarihi","type":"date","required":true},
                    {"id":"bitis_tarihi","label":"Bitiş Tarihi","type":"date","required":true},
                    {"id":"gun_sayisi","label":"Gün Sayısı","type":"number","required":true},
                    {"id":"aciklama","label":"Açıklama","type":"input","required":false}
                ]
                """
            },
            new()
            {
                Name = "Mağaza Sipariş Formu",
                Description = "Mağazadan ürün sipariş talebi",
                IsSystemForm = true,
                FieldsJson = """
                [
                    {"id":"urun_adi","label":"Ürün Adı","type":"input","required":true},
                    {"id":"urun_kategorisi","label":"Ürün Kategorisi","type":"select","required":true,"options":["Kesme Çiçek","Saksı Bitkisi","Aranjman","Aksesuar"]},
                    {"id":"miktar","label":"Miktar","type":"number","required":true},
                    {"id":"birim","label":"Birim","type":"select","required":true,"options":["Adet","Demet","Koli"]},
                    {"id":"istenen_tarih","label":"İstenen Tarih","type":"date","required":true},
                    {"id":"oncelik","label":"Öncelik","type":"select","required":true,"options":["Normal","Acil"]},
                    {"id":"notlar","label":"Notlar","type":"input","required":false}
                ]
                """
            },
            new()
            {
                Name = "Satın Alma Talebi Formu",
                Description = "Hammadde, ekipman veya malzeme satın alma talebi",
                IsSystemForm = true,
                FieldsJson = """
                [
                    {"id":"malzeme_adi","label":"Malzeme Adı","type":"input","required":true},
                    {"id":"malzeme_kategorisi","label":"Kategori","type":"select","required":true,"options":["Hammadde","Ambalaj","Kimyasal","Ekipman"]},
                    {"id":"miktar","label":"Miktar","type":"number","required":true},
                    {"id":"birim","label":"Birim","type":"select","required":true,"options":["Kg","Lt","Adet","Kutu"]},
                    {"id":"tahmini_tutar","label":"Tahmini Tutar (TL)","type":"number","required":true},
                    {"id":"tedarikci_onerisi","label":"Tedarikçi Önerisi","type":"input","required":false},
                    {"id":"aciklama","label":"Açıklama","type":"input","required":false}
                ]
                """
            },
            new()
            {
                Name = "Kalite Kontrol Raporu Formu",
                Description = "Ürün kalite değerlendirme ve kontrol raporu",
                IsSystemForm = true,
                FieldsJson = """
                [
                    {"id":"urun_adi","label":"Ürün Adı","type":"input","required":true},
                    {"id":"parti_no","label":"Parti No","type":"input","required":true},
                    {"id":"kontrol_tarihi","label":"Kontrol Tarihi","type":"date","required":true},
                    {"id":"gorunusel_durum","label":"Görünüsel Durum","type":"select","required":true,"options":["Uygun","Uygun Değil","Şartlı Uygun"]},
                    {"id":"boyut_kontrol","label":"Boyut Kontrol","type":"select","required":true,"options":["Uygun","Uygun Değil"]},
                    {"id":"renk_kontrol","label":"Renk Kontrol","type":"select","required":true,"options":["Uygun","Uygun Değil"]},
                    {"id":"genel_sonuc","label":"Genel Sonuç","type":"select","required":true,"options":["Kabul","Red","Şartlı Kabul"]},
                    {"id":"notlar","label":"Notlar","type":"input","required":false}
                ]
                """
            },

            // ── 11 Placeholder Form (henüz çalışmayan süreçler için) ──

            new() { Name = "Sera Üretim Emri Formu", Description = "Sera üretim planı ve emirleri", IsSystemForm = true, FieldsJson = """[{"id":"urun","label":"Ürün","type":"input","required":true},{"id":"miktar","label":"Miktar","type":"number","required":true},{"id":"tarih","label":"Üretim Tarihi","type":"date","required":true}]""" },
            new() { Name = "Depo Giriş/Çıkış Formu", Description = "Depo stok hareketi kayıt formu", IsSystemForm = true, FieldsJson = """[{"id":"hareket_tipi","label":"Hareket Tipi","type":"select","required":true,"options":["Giriş","Çıkış"]},{"id":"urun","label":"Ürün","type":"input","required":true},{"id":"miktar","label":"Miktar","type":"number","required":true}]""" },
            new() { Name = "Teslimat Planlama Formu", Description = "Teslimat rota ve zamanlama planı", IsSystemForm = true, FieldsJson = """[{"id":"hedef","label":"Hedef Adres","type":"input","required":true},{"id":"tarih","label":"Teslimat Tarihi","type":"date","required":true},{"id":"notlar","label":"Notlar","type":"input","required":false}]""" },
            new() { Name = "Fatura Onay Formu", Description = "Fatura ve ödeme onay süreci", IsSystemForm = true, FieldsJson = """[{"id":"fatura_no","label":"Fatura No","type":"input","required":true},{"id":"tutar","label":"Tutar (TL)","type":"number","required":true},{"id":"tedarikci","label":"Tedarikçi","type":"input","required":true}]""" },
            new() { Name = "Franchise Başvuru Formu", Description = "Yeni franchise başvuru ve değerlendirme", IsSystemForm = true, FieldsJson = """[{"id":"basvuran","label":"Başvuran","type":"input","required":true},{"id":"sehir","label":"Şehir","type":"input","required":true},{"id":"sermaye","label":"Sermaye (TL)","type":"number","required":true}]""" },
            new() { Name = "Franchise Denetim Formu", Description = "Franchise periyodik denetim raporu", IsSystemForm = true, FieldsJson = """[{"id":"magaza","label":"Mağaza","type":"input","required":true},{"id":"tarih","label":"Denetim Tarihi","type":"date","required":true},{"id":"sonuc","label":"Sonuç","type":"select","required":true,"options":["Başarılı","Eksik Var","Başarısız"]}]""" },
            new() { Name = "Bitki Sağlık Kontrol Formu", Description = "Bitki hastalık ve ilaçlama takibi", IsSystemForm = true, FieldsJson = """[{"id":"bitki","label":"Bitki Türü","type":"input","required":true},{"id":"durum","label":"Sağlık Durumu","type":"select","required":true,"options":["Sağlıklı","Hasta","Tedavi Altında"]},{"id":"notlar","label":"Notlar","type":"input","required":false}]""" },
            new() { Name = "İşe Alım Formu", Description = "Yeni personel talep ve değerlendirme", IsSystemForm = true, FieldsJson = """[{"id":"pozisyon","label":"Pozisyon","type":"input","required":true},{"id":"departman","label":"Departman","type":"input","required":true},{"id":"aciliyet","label":"Aciliyet","type":"select","required":true,"options":["Normal","Acil"]}]""" },
            new() { Name = "Demirbaş Talebi Formu", Description = "Ofis ekipmanı ve demirbaş talebi", IsSystemForm = true, FieldsJson = """[{"id":"demirba_adi","label":"Demirbaş Adı","type":"input","required":true},{"id":"miktar","label":"Miktar","type":"number","required":true},{"id":"gerekce","label":"Gerekçe","type":"input","required":true}]""" },
            new() { Name = "IT Destek Talebi Formu", Description = "Teknik sorun ve destek bildirimi", IsSystemForm = true, FieldsJson = """[{"id":"konu","label":"Konu","type":"input","required":true},{"id":"oncelik","label":"Öncelik","type":"select","required":true,"options":["Düşük","Normal","Yüksek","Kritik"]},{"id":"aciklama","label":"Açıklama","type":"input","required":true}]""" },
            new() { Name = "Sistem Erişim Talebi Formu", Description = "Sistem ve uygulama erişim yetki talebi", IsSystemForm = true, FieldsJson = """[{"id":"sistem","label":"Sistem/Uygulama","type":"input","required":true},{"id":"erisim_tipi","label":"Erişim Tipi","type":"select","required":true,"options":["Okuma","Yazma","Yönetici"]},{"id":"gerekce","label":"Gerekçe","type":"input","required":true}]""" },
        };
    }

    // ══════════════════════════════════════════
    //  WORKFLOW DEFINITIONS (15 süreç)
    // ══════════════════════════════════════════

    private static List<WorkflowDefinition> CreateWorkflowDefinitions(List<FormDefinition> forms)
    {
        var f = forms.ToDictionary(x => x.Name);

        return new List<WorkflowDefinition>
        {
            // A — Tedarik Zinciri (6 zincirleme: Sipariş → Satın Alma → Üretim → Depo → Teslimat → Fatura)
            new() { Name = "Mağaza Sipariş Talebi", Code = "MAGAZA_SIPARIS", Category = "TEDARIK", Description = "Mağazadan ürün sipariş süreci", FormTemplate = f["Mağaza Sipariş Formu"], NextWorkflowCode = "SATIN_ALMA_TALEBI" },
            new() { Name = "Satın Alma Talebi", Code = "SATIN_ALMA_TALEBI", Category = "TEDARIK", Description = "Hammadde ve malzeme satın alma süreci", FormTemplate = f["Satın Alma Talebi Formu"], NextWorkflowCode = "SERA_URETIM" },
            new() { Name = "Sera Üretim Emri", Code = "SERA_URETIM", Category = "TEDARIK", Description = "Sera üretim planlama ve emir süreci", FormTemplate = f["Sera Üretim Emri Formu"], NextWorkflowCode = "DEPO_GIRIS_CIKIS" },
            new() { Name = "Depo Giriş/Çıkış", Code = "DEPO_GIRIS_CIKIS", Category = "TEDARIK", Description = "Depo stok hareketi kayıt süreci", FormTemplate = f["Depo Giriş/Çıkış Formu"], NextWorkflowCode = "TESLIMAT_PLANLAMA" },
            new() { Name = "Teslimat Planlaması", Code = "TESLIMAT_PLANLAMA", Category = "TEDARIK", Description = "Teslimat rota ve zamanlama süreci", FormTemplate = f["Teslimat Planlama Formu"], NextWorkflowCode = "FATURA_ONAY" },
            new() { Name = "Fatura Onayı", Code = "FATURA_ONAY", Category = "TEDARIK", Description = "Fatura ve ödeme onay süreci", FormTemplate = f["Fatura Onay Formu"] },

            // B — Franchise (2)
            new() { Name = "Franchise Başvurusu", Code = "FRANCHISE_BASVURU", Category = "FRANCHISE", Description = "Yeni franchise başvuru değerlendirme süreci", FormTemplate = f["Franchise Başvuru Formu"] },
            new() { Name = "Franchise Denetim", Code = "FRANCHISE_DENETIM", Category = "FRANCHISE", Description = "Franchise periyodik denetim süreci", FormTemplate = f["Franchise Denetim Formu"] },

            // C — Sera/Üretim (2)
            new() { Name = "Bitki Sağlık Kontrolü", Code = "BITKI_SAGLIK", Category = "SERA", Description = "Bitki hastalık ve ilaçlama takip süreci", FormTemplate = f["Bitki Sağlık Kontrol Formu"] },
            new() { Name = "Kalite Kontrol Raporu", Code = "KALITE_KONTROL", Category = "SERA", Description = "Ürün kalite değerlendirme süreci", FormTemplate = f["Kalite Kontrol Raporu Formu"] },

            // D — İK/İdari (3)
            new() { Name = "İzin Talebi", Code = "IZIN_TALEBI", Category = "IK_IDARI", Description = "Personel izin talep ve onay süreci", FormTemplate = f["İzin Talebi Formu"] },
            new() { Name = "Personel İşe Alım", Code = "ISE_ALIM", Category = "IK_IDARI", Description = "Yeni personel talep ve değerlendirme süreci", FormTemplate = f["İşe Alım Formu"] },
            new() { Name = "Demirbaş Talebi", Code = "DEMIRBAS_TALEBI", Category = "IK_IDARI", Description = "Ofis ekipmanı ve demirbaş talep süreci", FormTemplate = f["Demirbaş Talebi Formu"] },

            // E — Teknik (2)
            new() { Name = "IT Destek Talebi", Code = "IT_DESTEK", Category = "TEKNIK", Description = "Teknik sorun ve destek bildirimi süreci", FormTemplate = f["IT Destek Talebi Formu"] },
            new() { Name = "Sistem Erişim Talebi", Code = "SISTEM_ERISIM", Category = "TEKNIK", Description = "Sistem ve uygulama erişim yetki süreci", FormTemplate = f["Sistem Erişim Talebi Formu"] },
        };
    }

    // ══════════════════════════════════════════
    //  WORKFLOW STEPS
    // ══════════════════════════════════════════

    private static List<WorkflowStep> CreateWorkflowSteps(
        List<WorkflowDefinition> workflows,
        Dictionary<string, Role> r)
    {
        var w = workflows.ToDictionary(x => x.Code);
        var steps = new List<WorkflowStep>();

        // ── İzin Talebi (WORKING) ──
        // Herkes başlatır → departman müdürü onaylar → (7+ gün) İK onaylar
        steps.AddRange(new[]
        {
            new WorkflowStep { WorkflowDefinition = w["IZIN_TALEBI"], StepOrder = 1, Name = "Departman Müdürü Onayı", AssignedRole = r["GENEL_MUDUR"], ActionType = "Approve", AssignmentRule = "DEPT_MANAGER" },
            new WorkflowStep { WorkflowDefinition = w["IZIN_TALEBI"], StepOrder = 2, Name = "İK Müdürü Onayı", AssignedRole = r["IK_MUDURU"], ActionType = "Approve", IsConditional = true, ConditionJson = """{"field":"gun_sayisi","operator":">=","value":7}""" },
        });

        // ── Mağaza Sipariş Talebi (WORKING) ──
        // Mağaza Müdürü başlatır → Bölge Müdürü onaylar → Satın Alma işler → Depo hazırlar
        steps.AddRange(new[]
        {
            new WorkflowStep { WorkflowDefinition = w["MAGAZA_SIPARIS"], StepOrder = 1, Name = "Bölge Müdürü Onayı", AssignedRole = r["BOLGE_MUDURU"], ActionType = "Approve" },
            new WorkflowStep { WorkflowDefinition = w["MAGAZA_SIPARIS"], StepOrder = 2, Name = "Satın Alma İşleme", AssignedRole = r["SATIN_ALMA_UZMANI"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["MAGAZA_SIPARIS"], StepOrder = 3, Name = "Depo Hazırlama", AssignedRole = r["DEPO_SORUMLUSU"], ActionType = "Complete" },
        });

        // ── Satın Alma Talebi (WORKING) ──
        // SA Uzmanı başlatır → SA Müdürü onaylar → (>50K) Finans onaylar → Direktör final onay
        steps.AddRange(new[]
        {
            new WorkflowStep { WorkflowDefinition = w["SATIN_ALMA_TALEBI"], StepOrder = 1, Name = "Satın Alma Müdürü Onayı", AssignedRole = r["SATIN_ALMA_MUDURU"], ActionType = "Approve" },
            new WorkflowStep { WorkflowDefinition = w["SATIN_ALMA_TALEBI"], StepOrder = 2, Name = "Finans Müdürü Onayı", AssignedRole = r["FINANS_MUDURU"], ActionType = "Approve", IsConditional = true, ConditionJson = """{"field":"tahmini_tutar","operator":">","value":50000}""" },
            new WorkflowStep { WorkflowDefinition = w["SATIN_ALMA_TALEBI"], StepOrder = 3, Name = "Direktör Onayı", AssignedRole = r["OPERASYON_DIREKTORU"], ActionType = "Approve" },
        });

        // ── Kalite Kontrol Raporu (WORKING) ──
        // KK Uzmanı başlatır → KK Müdürü inceler → Sera Müdürü değerlendirir
        steps.AddRange(new[]
        {
            new WorkflowStep { WorkflowDefinition = w["KALITE_KONTROL"], StepOrder = 1, Name = "KK Müdürü İnceleme", AssignedRole = r["KALITE_MUDURU"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["KALITE_KONTROL"], StepOrder = 2, Name = "Sera Müdürü Değerlendirme", AssignedRole = r["SERA_MUDURU"], ActionType = "Approve" },
        });

        // ── 11 Placeholder Süreç (temel 2 adım) ──

        steps.AddRange(new[]
        {
            new WorkflowStep { WorkflowDefinition = w["SERA_URETIM"], StepOrder = 1, Name = "Sera Müdürü Onayı", AssignedRole = r["SERA_MUDURU"], ActionType = "Approve" },
            new WorkflowStep { WorkflowDefinition = w["SERA_URETIM"], StepOrder = 2, Name = "Operasyon Onayı", AssignedRole = r["OPERASYON_DIREKTORU"], ActionType = "Approve" },

            new WorkflowStep { WorkflowDefinition = w["DEPO_GIRIS_CIKIS"], StepOrder = 1, Name = "Lojistik Müdürü Onayı", AssignedRole = r["LOJISTIK_MUDURU"], ActionType = "Approve" },

            new WorkflowStep { WorkflowDefinition = w["TESLIMAT_PLANLAMA"], StepOrder = 1, Name = "Lojistik Müdürü Onayı", AssignedRole = r["LOJISTIK_MUDURU"], ActionType = "Approve" },
            new WorkflowStep { WorkflowDefinition = w["TESLIMAT_PLANLAMA"], StepOrder = 2, Name = "Depo Sorumlusu Hazırlama", AssignedRole = r["DEPO_SORUMLUSU"], ActionType = "Complete" },

            new WorkflowStep { WorkflowDefinition = w["FATURA_ONAY"], StepOrder = 1, Name = "Muhasebe İnceleme", AssignedRole = r["MUHASEBE_UZMANI"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["FATURA_ONAY"], StepOrder = 2, Name = "Finans Müdürü Onayı", AssignedRole = r["FINANS_MUDURU"], ActionType = "Approve" },

            new WorkflowStep { WorkflowDefinition = w["FRANCHISE_BASVURU"], StepOrder = 1, Name = "Franchise Müdürü Değerlendirme", AssignedRole = r["FRANCHISE_MUDURU"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["FRANCHISE_BASVURU"], StepOrder = 2, Name = "Operasyon Direktörü Onayı", AssignedRole = r["OPERASYON_DIREKTORU"], ActionType = "Approve" },

            new WorkflowStep { WorkflowDefinition = w["FRANCHISE_DENETIM"], StepOrder = 1, Name = "Franchise Müdürü İnceleme", AssignedRole = r["FRANCHISE_MUDURU"], ActionType = "Review" },

            new WorkflowStep { WorkflowDefinition = w["BITKI_SAGLIK"], StepOrder = 1, Name = "Sera Şefi İnceleme", AssignedRole = r["SERA_SEFI"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["BITKI_SAGLIK"], StepOrder = 2, Name = "Sera Müdürü Onayı", AssignedRole = r["SERA_MUDURU"], ActionType = "Approve" },

            new WorkflowStep { WorkflowDefinition = w["ISE_ALIM"], StepOrder = 1, Name = "İK Müdürü Değerlendirme", AssignedRole = r["IK_MUDURU"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["ISE_ALIM"], StepOrder = 2, Name = "Departman Müdürü Onayı", AssignedRole = r["OPERASYON_DIREKTORU"], ActionType = "Approve" },

            new WorkflowStep { WorkflowDefinition = w["DEMIRBAS_TALEBI"], StepOrder = 1, Name = "İdari İşler İnceleme", AssignedRole = r["IDARI_ISLER_SORUMLUSU"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["DEMIRBAS_TALEBI"], StepOrder = 2, Name = "Finans Müdürü Onayı", AssignedRole = r["FINANS_MUDURU"], ActionType = "Approve" },

            new WorkflowStep { WorkflowDefinition = w["IT_DESTEK"], StepOrder = 1, Name = "IT Müdürü Atama", AssignedRole = r["IT_MUDURU"], ActionType = "Review" },
            new WorkflowStep { WorkflowDefinition = w["IT_DESTEK"], StepOrder = 2, Name = "Sistem Yöneticisi Çözüm", AssignedRole = r["SISTEM_YONETICISI"], ActionType = "Complete" },

            new WorkflowStep { WorkflowDefinition = w["SISTEM_ERISIM"], StepOrder = 1, Name = "IT Müdürü Onayı", AssignedRole = r["IT_MUDURU"], ActionType = "Approve" },
            new WorkflowStep { WorkflowDefinition = w["SISTEM_ERISIM"], StepOrder = 2, Name = "Sistem Yöneticisi Uygulama", AssignedRole = r["SISTEM_YONETICISI"], ActionType = "Complete" },
        });

        return steps;
    }
}
