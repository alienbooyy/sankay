# Sankay POS - Hızlı Başlangıç Kılavuzu

## Kurulum

1. Projeyi klonlayın veya indirin
2. Visual Studio 2022 veya daha yeni bir sürümle açın
3. F5'e basarak uygulamayı başlatın

## İlk Kullanım

### Adım 1: Uygulamayı Başlatın
Uygulama ilk başlatıldığında:
- Veritabanı otomatik oluşturulur
- 12 masa varsayılan olarak eklenir
- Örnek ürünler ve hammaddeler yüklenir
- Yönetici şifresi: **1234**

### Adım 2: Masaları Kontrol Edin
- Ana ekranda yeşil masalar boş, kırmızı masalar doludur
- Bir masaya tıklayarak sipariş ekranını açın

### Adım 3: İlk Siparişi Alın
1. Bir masaya tıklayın
2. Sol panelden ürünleri seçin (örn: Lahmacun, Ayran)
3. Sağ panelde sipariş özeti görünür
4. "Ödeme Al" butonuna basarak siparişi tamamlayın

### Adım 4: Yönetim Panellerini Keşfedin
- **Yönetici Paneli**: Günlük raporlar ve Excel dışa aktarımı
- **Masa Yönetimi**: Masa ekle/sil/düzenle
- **Ürün Yönetimi**: Menü ürünlerini yönetin
- **Stok Yönetimi**: Hammadde ve envanter takibi
- **Reçete Yönetimi**: Ürünlerin içeriklerini tanımlayın

## Temel Özellikler

### Sipariş Alma
1. Masaya tıklayın
2. Ürün seçin (sol panel)
3. Sipariş listesinde çift tıklayarak ürün silin
4. "Ödeme Al" ile siparişi tamamlayın

### Alman Usulü Ödeme
- Hesabı bölmek için "Alman Usulü" butonunu kullanın
- Her kişinin ödeyeceği tutarı girin
- Kalan tutar otomatik hesaplanır

### Rapor Alma
1. "Yönetici Paneli"ne girin (şifre: 1234)
2. Tarih aralığı seçin
3. "Göster" ile raporu görüntüleyin
4. "Excel'e Aktar" ile Excel dosyası oluşturun

### Stok Yönetimi
- Düşük stoklar kırmızı uyarı ile gösterilir
- Sipariş tamamlandığında stok otomatik düşer
- Minimum stok seviyesi ayarlanabilir

## Sık Sorulan Sorular

**S: Yönetici şifresi nedir?**  
C: Varsayılan şifre: **1234**. Değiştirmek için veritabanındaki Settings tablosunu düzenleyin.

**S: Örnek ürünleri nasıl değiştiririm?**  
C: "Ürün Yönetimi" panelinden mevcut ürünleri düzenleyin veya silin, yeni ürünler ekleyin.

**S: Yazıcı nasıl çalışır?**  
C: Şu anda yazdırma Debug konsoluna gider. SPENTA yazıcı entegrasyonu için SDK gereklidir.

**S: Tablet nasıl bağlanır?**  
C: Tablet sunucusu TabletServerEnabled ayarı ile etkinleştirilebilir. JSON formatında sipariş gönderilir.

**S: Veritabanı nerede?**  
C: Uygulama klasöründe `sankay.db` dosyası olarak oluşturulur.

## Destek

Sorularınız için GitHub'da issue açabilirsiniz.
