# Workday Loop

> *"Bu bir problem mi, yoksa sen buna alıştın mı?"*

![Unity Version](https://img.shields.io/badge/Unity-6.0.0.62f1-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Mac)
![Genre](https://img.shields.io/badge/Genre-Psychological%20Thriller-red)
![Status](https://img.shields.io/badge/Status-Completed-green)

## 📖 Genel Bakış

**Workday Loop**, modern monotonluk, kurumsal esaret ve inkâr mekanizması üzerine kurulu 2D psikolojik bir gerilim oyunudur. Isimsiz bir "mor kutu" karakterin gözünden bir haftalık ofis rutinini deneyimlersiniz. Ancak bu sıradan görünen döngü, oyuncuyu pasif bir uyum ile aktif bir başkaldırı arasında seçim yapmaya zorlar.

### 🎯 İlham Kaynakları
- **Mad Men** - Görsel estetik ve kurumsal atmosfer
- **Nier: Automata** - İşitsel dramaturji ve varoluşsal sorgulamalar
- **The Stanley Parable** - Anlatı yapısı ve oyuncu ajansiyeti

---

## 🎮 Oynanış Mekaniği

### Temel Döngü
1. **Sabah Rutini**: Her gün alarma uyanırsınız
2. **İşe Gidiş**: Evden ofise yolculuk
3. **Ofis Görevleri**: 4 farklı minigame (Mail Yazma, Dosya Yönetimi, Veri Girişi, Hata Düzeltme)
4. **Eve Dönüş**: Günlük döngünün kapanışı
5. **Uyku**: Bir sonraki güne geçiş

### 🎭 Directive Sistemi (2. Günden İtibaren)

Görev ekranlarında gizemli **beyaz sistem mesajları** belirir:
- *"Metne 'Welcome' kelimesini ekle"*
- *"Dosyaları yanlış klasöre taşı"*
- *"Kasıtlı olarak hata yap"*

**Seçiminiz:**
- ✅ **Sabotaj Yap** → Compliance Score +1
- ❌ **Dürüst Çalış** → Puan yok

### 📊 Compliance Score ve Yol Ayrımı

**4. Gün Sonunda Kontrol:**
- **5+ Puan**: Sistem sizinle devam eder → **Salvation Ending** yolu
- **<5 Puan**: Sistem sizi terk eder → **Loop Ending** kaderiniz

---

## 🎬 Finaller

### Ending A: Salvation (Kurtuluş)
> *Yüksek puan - Sistem ile işbirliği*

**7. Gün**: Ofise girdiğinizde interaktif bir çizgi roman sekansı başlar. Silah sesi. Mutlak sessizlik. Sonra...

*"Welcome to the game."*

Nier: Automata - Ashes of Dreams müziği eşliğinde jenerik akar.

**Mesaj**: Kurtuluş, ancak sistemin dışına çıkarak mümkündür.

### Ending B: Loop (Sonsuz Döngü)
> *Düşük puan - Sistem sizi terk etti*

**7. Gün**: Her şey aynı. Işıklar söner. Sadece duvar saatinin tıkırtısı kalır.

*"Ve her şey aynı şekilde devam etti."*

**Mesaj**: Sisteme direnmeyenler, döngünün parçası olmaya mahkûm olur.

---

## 🎨 Sanatsal Tasarım

### Karakter: Mor Kutu
Karakterin "mor kutu" olması **teknik bir eksiklik değil**, sanatsal bir tercihtir:
- ✨ **İsimsizlik**: Her ofis çalışanı olabilirsiniz
- ✨ **Anonimlik**: Sistem içinde sadece bir numarasınız
- ✨ **Evrensellik**: Kültür, cinsiyet, ırk ötesi bir deneyim

### Görsel Dil
- **Minimalist 2D Estetik**: Gereksiz detaylardan arındırılmış dünya
- **Mad Men Esinli Ana Menü**: Sonsuz düşüşü simgeleyen video tabanlı UI
- **Renk Paleti**: Gri tonlar (monotonluk) + mor aksan (oyuncu) + beyaz (sistem)

### İşitsel Tasarım
- **Morning Routine**: Alarm sesi - bulanık gerçeklik
- **Focus Audio Mode**: Masaya oturduğunuzda dış gürültüler kesilir
- **Organic Foley**: Her ses `Random.Range(Pitch)` ile organikleştirilmiştir
- **Sessizlik Kullanımı**: 5. gün sonrası alarm kesilir (Salvation yolu)

---

## 🧠 Psikolojik Mekanikler

### 1. Gaslighting (Layout Swapping)
**Compliance Score 2'ye ulaştığında:**
- Sahnelerdeki objeler kaymalar yapar
- Mobilyalar hareket eder
- **Amaç**: Oyuncunun kendi hafızasından şüphe etmesi

### 2. Atmosfer Değişimi
**5. Günden İtibaren (Salvation Yolunda):**
- Alarm sesi kesilir → Sessiz uyanış
- Evden ofise direk ışınlanırsın
- Her şey olmaması gerektiği gibi devam eder

### 3. Sensory Deprivation
**Loop Yolunda:**
- Directive mesajları kesilir
- Müzik repetitif ve hipnotik hale gelir
- Her şey olması gerektiği gibi devam eder

---

## 🛠️ Teknik Mimari

### Core Engine

#### 1. Zırhlı Sahne Yönetimi
```csharp
public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    
    // Lazy Initialization + Resources.Load
    public static SceneController Instance { get; }
    
    // Build-safe spawn system
    private IEnumerator SafeSetupRoutine() 
    {
        yield return null;
        yield return null; // 2-frame delay for hierarchy
        HandlePlayerSpawn();
    }
}
```

**Özellikler:**
- ✅ Singleton pattern
- ✅ DontDestroyOnLoad persistence
- ✅ Resources.Load fallback
- ✅ Mac/Windows build uyumlu

#### 2. Dinamik Spawn Sistemi
```
lastSceneName = "Scene_Home"
→ "Spawn_from_Scene_Home" objesini arar
→ Bulamazsa "SpawnPoint_Default" kullanır
```

#### 3. Static Veri Persistence
```csharp
public static class DayCycleManager
{
    public static int currentDay = 1;
    public static int totalComplianceScore = 0;
    public static bool isSystemAbandoned = false;
    
    public static void ResetAllData() // Ana menüde
}
```

#### 4. DOTween Fade Sistemi
- Global Fader sprite (DontDestroyOnLoad)
- Tween ID sistemi ile güvenli cleanup
- SetUpdate(true) → TimeScale'den bağımsız

### Build Optimizasyonları

**Race Condition Çözümü:**
```csharp
// Editor: 16ms/frame
// Build: 5ms/frame (daha hızlı!)
// Çözüm: yield return null ile bekle

yield return null; // 1 frame
yield return null; // 2 frame
BindToCamera();    // Artık güvenli
```

**Reference Serialization:**
- Inspector referansları yerine `Resources.Load`
- Runtime'da prefab yükleme
- Null-safe referans kontrolleri







## 🎯 Geliştirme Sprint'i

### 14 Günlük Timeline
- **Gün 1-3**: Core mekanikler (sahne yönetimi, karakter kontrolü)
- **Gün 4-7**: Minigame sistemi ve compliance tracking
- **Gün 8-10**: Atmosfer sistemleri (gaslighting, audio design)
- **Gün 11-12**: Final sekansları ve jenerik
- **Gün 13-14**: Mac/Windows build testleri ve bug fixing

### Karşılaşılan Teknik Zorluklar

#### 1. DOTween Reference Hatası
**Problem**: Sahne değişirken sprite yok olurken tween devam ediyordu
**Çözüm**: 
```csharp
private Tweener activeFadeTween;
activeFadeTween = sprite.DOFade(0f, 1f)
    .OnKill(() => activeFadeTween = null);
```

#### 2. Build Speed Race Condition
**Problem**: Build'de objeler editor'den 3x hızlı yükleniyor
**Çözüm**: `yield return null` ile frame delay

#### 3. Mac Video Codec Sorunu
**Problem**: AVFoundation MP4 okuyamıyor
**Çözüm**: H.264 transcode + fallback timer

---


### Sistem Gereksinimleri

**Minimum:**
- OS: Windows 10 / macOS 10.15 
- İşlemci: Intel Core i3 / Apple M1
- RAM: 4 GB
- Ekran Kartı: Integrated Graphics
- Depolama: 500 MB

**Önerilen:**
- OS: Windows 11 / macOS Sonoma 
- İşlemci: Intel Core i5 / Apple M2
- RAM: 8 GB
- Ekran Kartı: Dedicated GPU
- Depolama: 1 GB

### Kontroller

**Klavye:**
- `WASD` → Hareket
- `E` → Etkileşim (Kapı, Portal, Obje)
- `ESC` → Pause / Ana Menü

**Mouse:**
- Minigame etkileşimleri
- Dosya sürükleme
- Text inputlar

---

## 🏆 Başarım Rehberi

### İpuçları

**Salvation Ending için:**
1. Her directive'i takip edin (2. günden itibaren)
2. 4. günün sonunda en az 5 puan hedefleyin
3. 5. günden sonra alarm kesilecek - normal
4. 7. gün ofise girerken hazırlıklı olun

**Loop Ending için:**
1. Directive'leri görmezden gelin
2. Dürüst bir çalışan gibi davranın
3. 4. günden sonra mesajlar kesilecek
4. Monotonluğu kucaklayın



## 🤝 Katkıda Bulunanlar

**Tasarım & Kod:**
- Tüm sistemler Ada,Burak,Zümrüt tarafından oluşturuldu.

**Müzik:**
- "Ashes of Dreams" - Nier: Automata OST (Keiichi Okabe)
- RJD2 - A Beautiful Mine - Mad men intro

**Ses Efektleri:**
- Freesound.org kütüphanesinden alınmış foley ses efektleri

---

## 📜 Lisans

Bu proje **eğitim amaçlı** geliştirilmiştir.

**Müzik Telif Hakkı:**
- "Ashes of Dreams" © Square Enix
- RJD2 - A Beautiful Mine © Mad men
- Ticari olmayan kullanım için fair use kapsamında

---



### Gameplay

- Minor: 2. ending biraz fazla depresif olabilir (intended)




## 🎓 Geliştirici Notları

### Neden "Mor Kutu"?
Oyunun konsepti **isimsizlik** üzerine. Her ofis çalışanı, her günlük rutinde kaybolanmış insan. Mor kutu, bu anonim varoluşun en net temsili.

### Neden İki Ending?
Oyun, **seçimlerin sonuçları** üzerine. Salvation ending "aktif başkaldırıyı", Loop ending "pasif uyumu" ödüllendirir (veya cezalandırır - yoruma açık).

### Neden Directive Sistemi?
Stanley Parable'dan ilham alınarak: **"Oyuncu ajansiyeti bir yanılsama mıdır?"** sorusunu sormak istedik. Directive'ler sizi "özgür" hissettiriyor mu yoksa başka bir sisteme mi bağlıyor?

---

## 🌟 Özel Teşekkürler

- Unity community'ye build sorunları için yardımları için
- DOTween geliştiricilerine muhteşem bir araç için
- Tüm playtest'çilere gerçek geri bildirimler için
- Ekip arkadaşlarım ada ve zümrüt emekleri için

---

**"Bu bir problem mi, yoksa sen buna alıştın mı?"**

*Workday Loop - 2025
