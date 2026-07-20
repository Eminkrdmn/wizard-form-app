# Wizard Form App

Çok adımlı (wizard) form tasarımı ve süreç yönetimi uygulaması. Kullanıcılar form tasarlar, tasarlanan formlar üzerinden süreç başlatır ve süreçleri state machine mantığıyla yönetir.

## Kurulum

```bash
git clone https://github.com/Eminkrdmn/wizard-form-app.git
cd wizard-form-app
npm install
npm run dev
```

Uygulama `http://localhost:3000` adresinde çalışır.

## Varsayılan Kullanıcılar

| Kullanıcı adı | Şifre | Rol   |
| ------------- | ----- | ----- |
| admin         | admin | admin |
| user          | user  | user  |

**Rol farkı:** Form Tasarımı ekranı yalnızca `admin` rolüne açıktır (menüde görünmez + sayfa düzeyinde RoleGuard koruması). Diğer ekranlar her iki role de açıktır.

## Uygulama Akışı

1. **Login** → kullanıcı ve rol bilgisi global state'e (Zustand, persist) alınır
2. **Form Tasarımı (admin):** alan ekleme (input, number, select, checkbox, date), etiket/tip/zorunluluk, select seçenekleri, bağımlı zorunluluk kuralı ("A alanında X seçilirse B zorunlu olur"), sürükle-bırak ile alan sıralama
3. **Süreç Başlatma:** tasarlanan form dinamik olarak render edilir (DynamicForm), veri girilir, validasyondan geçerse mock API üzerinden süreç oluşturulur
4. **Süreç Detayı:** süreç ID, durum, tarihler, form verileri, JSON çıktısı, aksiyon butonları ve süreç geçmişi
5. **Dashboard:** süreçlerin bekleyen / devam eden / tamamlanan olarak kategorize görünümü

## Süreç Yönetimi (State Machine)

Durumlar ve geçişler `lib/stateMachine.ts` içinde statik olarak tanımlıdır:

```
Beklemede ──► DevamEdiyor ──► Tamamlandi
    │              │
    └──────────────┴────────► Reddedildi
```

`Tamamlandi` ve `Reddedildi` terminal durumlardır. Her durum değişikliği kim/ne zaman bilgisiyle süreç geçmişine loglanır.

## Validasyon

`lib/validation.ts` içindeki saf fonksiyon üç katman kontrol eder:

- **Zorunlu alan** kontrolü
- **Tip bazlı** kontrol (number, date)
- **Bağımlı zorunluluk:** başka bir alanın seçimine göre koşullu zorunluluk

## Teknolojiler

- **Next.js 16** (App Router) + **React 19** + **TypeScript**
- **Zustand** — global state (auth, form, süreç, ayarlar) + persist middleware
- **Tailwind CSS v4** — design token tabanlı tema sistemi (CSS değişkenleri ile açık/koyu tema)
- **react-i18next** — TR/EN dil desteği
- **@dnd-kit** — sürükle-bırak alan sıralama
- **Mock API** — Next.js Route Handlers (`app/api/*`): login, form CRUD, süreç oluşturma; loading/success/error state'leri gerçek backend'e bağlanıyormuş gibi yönetilir

## Mimari Notlar

- `components/ui/` — custom Input, Select, Checkbox, Button componentleri (tüm formlarda ortak)
- `components/form-renderer/DynamicForm.tsx` — form tanımını (JSON) ekrana çeviren controlled component
- `stores/` — Zustand store'ları; state güncellemeleri immutable
- `lib/` — UI'sız mantık: validasyon, state machine, i18n kurulumu
- Route koruması: login kontrolü `(dashboard)/layout.tsx`'te, rol kontrolü `RoleGuard` componentiyle sayfa düzeyinde

## Kapsam Notları

- Backend bilinçli olarak **mock API** ile kurgulanmıştır (dokümandaki ilgili not uyarınca değerlendirme frontend kapsamındadır). Mock API veriyi bellekte tutar; kalıcılık frontend tarafında Zustand persist (localStorage) ile sağlanır.
- Süreç durumları, aksiyon etiketleri ve API mesajları veri katmanına ait olduğundan çeviri kapsamı dışında bırakılmıştır; tüm arayüz metinleri TR/EN çevrilidir.
- Süreç tasarımı drag&drop'u (Big Bonus) ve .NET backend kapsam dışıdır.
