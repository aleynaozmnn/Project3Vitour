# 🌍 Vitour - Modern Travel & Destination Management System

Vitour; tur rotalarının, destinasyon bilgilerinin ve kullanıcı etkileşimlerinin dinamik olarak yönetildiği, NoSQL mimarisiyle güçlendirilmiş profesyonel bir ASP.NET Core 8.0 Fullstack projesidir.

## 🚀 Proje Öne Çıkanları

MongoDB Entegrasyonu: Esnek ve yüksek performanslı veri yönetimi için NoSQL mimarisi tercih edildi.

Çoklu Dil Desteği (Localization): IStringLocalizer kullanılarak sistemin tamamı Türkçe ve İngilizce dil seçeneklerine uyumlu hale getirildi.

Dinamik Admin Paneli: Sidebar ve içerik alanının dikeyde %100 uyumlu olduğu, modern ve responsive bir dashboard tasarımı geliştirildi.

Gelişmiş Veri Tabloları: DataTables entegrasyonu ile anlık arama, dinamik sıralama ve sayfalama özellikleri eklendi.

Excel Raporlama: Sistemdeki tüm tur verilerinin tek tıkla Excel formatına dönüştürülmesi sağlandı.

Güvenli E-Posta Servisi: MailKit ve Google App Password altyapısı ile güvenli bildirim gönderimi sağlandı. (Hassas veriler User Secrets ile korunmaktadır.)

## 🛠 Teknik Yetkinlikler & Kütüphaneler

### 💾 Veri Yönetimi

MongoDB.Driver (2.30.0): NoSQL veritabanı iletişimi için.

AutoMapper (13.0.1): Nesne eşleme (Entity-DTO) süreçlerini otomatize etmek için.

### 📧 İletişim & Raporlama

MailKit (4.16.0): SMTP tabanlı güvenli e-posta gönderimi için.

ClosedXML (0.104.2): Verileri Excel formatında raporlamak için.

### 🎨 UI & UX (Kullanıcı Deneyimi)

X.PagedList.Mvc.Core (9.1.2): Performanslı sayfalama yapısı için.

SweetAlert2: Modern ve interaktif kullanıcı bildirimleri (Pop-up) için.

DataTables.net: Dinamik ve yönetilebilir tablolar için.

### 🔐 Güvenlik

Secret Manager (User Secrets): API anahtarları ve şifrelerin kod tabanından ayrıştırılması için kullanılan modern yaklaşım.

📸 Ekran Görüntüleri

### ⚙️ Kurulum ve Yapılandırma

Projeyi klonlayın:

git clone [https://github.com/aleynaozmnn/Project3Vitour.git](https://github.com/aleynaozmnn/Project3Vitour.git)


MongoDB Ayarı: appsettings.json dosyasındaki Connection String bilgisini kendi MongoDB adresinizle güncelleyin.

Güvenlik (Mail): Mail gönderimi için Google App Password şifrenizi User Secrets üzerinden tanımlayın:

{
  "MailPassword": "sizin_16_haneli_kodunuz"
}


Çalıştırın: Projeyi Visual Studio üzerinden F5 ile veya terminalden dotnet run komutuyla başlatın.

⭐ Bu proje, modern yazılım mimarilerini ve NoSQL teknolojilerini gerçek dünya senaryolarıyla birleştirmek amacıyla geliştirilmiştir.

## 📸 Ekran Görüntüleri
### <img width="1910" height="1680" alt="turListesi(1)" src="https://github.com/user-attachments/assets/36ad4138-1a57-4957-b410-01b5233b9049" />
Tur-Seyehat sitesi giriş sayfasıdır.



### <img width="1910" height="1595" alt="turPlani" src="https://github.com/user-attachments/assets/59d9e07b-0001-4916-a4ee-4b3fa4d7e9d2" />
Tura ait gün planları bu sekmede bulunur.

### <img width="1910" height="1663" alt="turKonumHaritasi" src="https://github.com/user-attachments/assets/d8201271-c1d7-4ab8-9dcc-71a1170bb9a3" />
Tura ait konum haritası bu sekmede bulunur.

### <img width="1910" height="1623" alt="turYorumSayfasi" src="https://github.com/user-attachments/assets/e148c1fa-080c-4a63-9a17-a8900206adef" />
Tur hakkında yapılan yorumlar bu sekmede bulunur.

### <img width="1910" height="1594" alt="turGaleriSayfasi" src="https://github.com/user-attachments/assets/40fd9d36-e16b-4725-9fd9-0b650ea5be12" />
Tur için admin tarafından eklenen görseller bu sekmede bulunur.

### <img width="1889" height="895" alt="image" src="https://github.com/user-attachments/assets/a8a4c6fc-4667-4727-b43e-4b47c3b1ac03" />
Eğer ilgili turun rezervasyon durumu varsa,önceki sayfada bulunan Rezervasyon Yap butonu aktiftir.Gezgin oraya basarak bu sayfaya yönlendirilir.

### <img width="1886" height="891" alt="baliRezervasyonSayfasi" src="https://github.com/user-attachments/assets/f8423f9e-5e2f-44fc-96ae-2d161f505d0f" />
Rezervasyon başarılı ise bildirim gösterilir.

### <img width="828" height="1361" alt="WhatsApp Image 2026-04-30 at 12 22 34" src="https://github.com/user-attachments/assets/b93b9016-ea6a-4b8c-8d74-245f8c53e9b1" />
Rezervasyonun yapıldığına dair gezgine onay maili gönderilir.

### <img width="1873" height="878" alt="adminGiris_sayfasi" src="https://github.com/user-attachments/assets/57a25893-58f9-4e2a-b935-ec0b2688b1d2" />
Veritabanına kayıtlı admin bilgileri(Username:Özmen|password:aleyna12345)bilgileri girilerek admin olduğunu onaylanmak amacıyla kayıtlı mail adresine onay maili gönderilir.

### <img width="611" height="363" alt="giris_mail_dogrulma" src="https://github.com/user-attachments/assets/c79fc17e-dbd2-461b-a0b7-aa1d1142e034" />
Doğrulama kodu girildikten sonra eğer doğruysa admin panele giriş yapar.



### <img width="1878" height="911" alt="PatronYonetimpaneliGiris" src="https://github.com/user-attachments/assets/e443f44c-2008-4f46-bac6-7d98df4cc401" />
Admin bu sekmeden tur bazlı kontenjan analizi yapabilir. Turlar ve planları üzerinde CRUD işlemi gerçekleştirebilir.Genel bazlı istatistikler görüntüleyebilir.

### <img width="1893" height="894" alt="PatronPaneli(yeni_tur_ekle)" src="https://github.com/user-attachments/assets/73e39aea-7d70-4bbf-93ae-d3ca539eb61e" />


### <img width="1883" height="906" alt="patronPaneli(tur_detay_sayfasi)" src="https://github.com/user-attachments/assets/1915bcd1-e403-48fa-8c1a-e729206f62a5" />
Admin burdan ilgili tura ait detaylara erişebilir.

### <img width="1882" height="903" alt="patronPaneli(tur_palnini_yonetim_sayfasi)" src="https://github.com/user-attachments/assets/5d80f8cf-1a04-48c1-a08d-2b1f5b303997" />
Tur detay sayfasının en altındaki tur planını yönet butonu bu sayfaya yönlendirir ilgili turun planı üzerinde değişiklikler yapılabilir.

### <img width="1885" height="904" alt="image" src="https://github.com/user-attachments/assets/efbc23af-bf32-43f1-8b96-e5bc0b66b8a9" />
Turlarımız sekmesinden aktif/pasif turların tamamına erişim sağlanabilir.Fiyat veya kategori bazlı filtrelemeler yapılabileceği gibi, gezginin turlar sekmesi altında bulunan galeri alt sekmesine görsel eklenme işlemi de buradan yapılır.

### <img width="1878" height="899" alt="image" src="https://github.com/user-attachments/assets/c25b2709-fe32-4c4a-abcb-b89eac9bdf88" />

### <img width="1893" height="905" alt="image" src="https://github.com/user-attachments/assets/a032d032-c2f6-443a-9cf9-7077cb5c5f9e" />

### <img width="1863" height="891" alt="rezervasyonListeleme" src="https://github.com/user-attachments/assets/92204333-4bd8-4209-9829-bef2a15b3d5b" />
Yapılan tüm rezervasyonlara erişim buradan sağlanır.Excel çıktısı da alınabilir.Rezervasyon detayına erişilebilir.

### <img width="1910" height="915" alt="screencapture-localhost-7206-AdminTour-ReservationDetail-69e8ac383b0b88d33837733f-2026-04-30-21_04_04" src="https://github.com/user-attachments/assets/5e8ec924-787c-461a-8b4f-fe0002f87e27" />

### <img width="1910" height="915" alt="kategoriYonetimi" src="https://github.com/user-attachments/assets/33cccb4c-2bc0-4050-ba42-6720eb54ea76" />
Turların ilgili olduğu alt kategori üzerindeki işlemler bu sayfadadır.Kategoriler üzerinde CRUD yapılabilir

### <img width="1910" height="915" alt="screencapture-localhost-7206-Category-Index-2026-04-30-21_05_32" src="https://github.com/user-attachments/assets/7849303e-81ad-428f-a636-696fc92251aa" />

### <img width="1873" height="906" alt="destinasyonYonetimi" src="https://github.com/user-attachments/assets/21b35535-a2e6-4613-a9bb-80830a63ebe3" />
Destinasyon sekmesi sayesinde tura ait genel(ülke)bilgileri,kapasite bilgileri gibi rota bazlı gösterim yapılır.

### <img width="1910" height="915" alt="screencapture-localhost-7206-Destination-CreateDestination-2026-04-30-21_06_35" src="https://github.com/user-attachments/assets/6505eb42-c02b-41d5-aaec-c4c1d5b150b4" />

### <img width="1910" height="915" alt="screencapture-localhost-7206-Review-ReviewList-2026-04-30-21_10_19" src="https://github.com/user-attachments/assets/812284ba-7830-4efa-8545-0d5d393dbbdc" />
Tura gelen yorumlar burada listelenir.Adminden onay bekleyenler ve aktif yorumalr olarak.

### <img width="1890" height="905" alt="ayarlarSayfasi" src="https://github.com/user-attachments/assets/20834d2f-0aab-46a3-81f8-ad13f3b6defa" />
Ayarlar sayfasıdır.Dil değişimi(ayarlar+sidebar dahil)yapılabilir.

