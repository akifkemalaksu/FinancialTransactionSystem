# Financial Transaction System (Finansal Transfer Sistemi)

Bu proje, hesaplar arası para transferlerini güvenli, ölçeklenebilir ve dağıtık bir yapıda gerçekleştiren, mikroservis mimarisi ile geliştirilmiş bir finansal işlem sistemidir.

Proje, **Event-Driven Architecture (Olay Güdümlü Mimari)** ve **CQRS** desenlerini kullanır. Servisler arası iletişim **Kafka** üzerinden asenkron olarak sağlanır ve veri tutarlılığı için **Outbox/Inbox** desenleri uygulanmıştır.

## 🚀 Teknolojiler ve Mimari

Bu proje aşağıdaki modern teknolojileri ve desenleri barındırır:

*   **Platform:** .NET 10
*   **Mimari:** Microservices, Clean Architecture, CQRS
*   **Veritabanı:** PostgreSQL (Entity Framework Core)
*   **Ön Bellekleme (Caching):** Redis
*   **Mesajlaşma (Messaging):** Apache Kafka (MassTransit ile)
*   **Resilience (Dayanıklılık):** Polly
*   **Containerization:** Docker & Docker Compose
*   **Monitoring & Metrics:** Prometheus
*   **Log Yönetimi:** Loki
*   **Distributed Tracing:** Tempo
*   **Görselleştirme ve Dashboard:** Grafana

## 🏗️ Servisler

Sistem aşağıdaki temel mikroservislerden oluşur:

1.  **AccountService:** Kullanıcı hesaplarını yönetir, bakiye güncellemelerini yapar.
2.  **FraudDetectionService:** Transfer işlemlerini şüpheli aktivitelere karşı denetler.
3.  **LedgerService:** Tüm finansal işlemlerin muhasebe kayıtlarını (defter-i kebir) tutar.
4.  **NotificationService:** İşlem durumları hakkında (başarılı/başarısız) bildirimleri yönetir.
5.  **TransactionService:** Transfer sürecini başlatan ve koordine eden servistir.

## 🛠️ Kurulum ve Çalıştırma (Docker)

Projeyi ayağa kaldırmak için **Docker** ve **Docker Compose** gereklidir.

### Sistemi Başlatma

Aşağıdaki komut ile tüm servisleri, veritabanlarını ve Kafka'yı derleyip arka planda başlatabilirsiniz:

```bash
docker compose up --build -d
```

Bu komut şunları yapar:
*   PostgreSQL, Redis ve Kafka konteynerlerini başlatır.
*   Gerekli Kafka topic'lerini (`transfer-created`, `transfer-completed`, `transfer-failed`) oluşturur.
*   Tüm mikroservisleri (.NET 10) build eder ve çalıştırır.

### Sistemi Durdurma

Tüm konteynerleri durdurmak ve silmek için:

```bash
docker compose down
```

## 🔍 Servis Portları

Docker üzerinde çalışan servislerin dışarıya açılan portları:

*   **Account Service:** 5001
*   **Fraud Detection Service:** 5002
*   **Ledger Service:** 5003
*   **Notification Service:** 5004
*   **Transaction Service:** 5005
*   **PostgreSQL:** 5101
*   **Redis:** 5102
*   **Kafka:** 5103
*   **Grafana:** 3000
*   **Prometheus:** 9090
*   **Loki:** 3100
*   **Tempo:** 3200

## 📈 Monitoring & Observability (Grafana)

Sistem ayağa kalktıktan sonra log, metrik ve trace takibi için **Grafana** kullanılmaktadır.

### 1. Grafana Erişimi

Konteynerler çalışmaya başladıktan sonra tarayıcınızdan aşağıdaki adrese gidin:

- URL: `http://localhost:3000`
- Kullanıcı Adı: `admin` (varsayılan)
- Şifre: `admin` (varsayılan – ilk girişte değiştirmeniz istenebilir)

### 2. Veri Kaynaklarını (Data Sources) Ekleme

Grafana içerisinde verileri görebilmek için aşağıdaki veri kaynaklarını **Connections > Data Sources** menüsünden tek tek eklemelisiniz:

| Veri Kaynağı | URL                     | Açıklama                                           |
|-------------|-------------------------|----------------------------------------------------|
| Prometheus  | `http://prometheus:9090`| Metrik verileri (CPU, RAM, Request Count vb.)      |
| Loki        | `http://loki:3100`      | Log verileri (Application & Container Logs)        |
| Tempo       | `http://tempo:3200`     | Dağıtık izleme verileri (Distributed Tracing)      |

### 3. Log ve Trace İlişkilendirmesi (Correlation)

Logların içindeki `trace_id` üzerinden doğrudan ilgili trace görüntüsüne zıplamak için Loki veri kaynağında aşağıdaki ayarı yapın:

1. Grafana'da **Data Sources > Loki** ayarlarına girin.
2. **Derived Fields** bölümüne gidin ve **Add** butonuna basın.
3. Aşağıdaki alanları doldurun:
   - Name: `TraceID`
   - Regex: `(?:trace_id|tid)=(\\w+)`
   - Internal link: **On**
   - Internal link target: Tempo veri kaynağını seçin.
4. **Save & Test** diyerek değişiklikleri kaydedin.

### 4. Verileri Görüntüleme (Explore)

Sol menüdeki **Explore** (pusula simgesi) sekmesine tıklayarak gerçek zamanlı log, metrik ve trace sorguları yapabilirsiniz:

- **Loglar için:** Veri kaynağını **Loki** seçin, `container_name` ya da servis etiketlerine göre filtreleyin.
- **Trace’ler için:** Veri kaynağını **Tempo** seçin, **Search** sekmesiyle mikroservisler arası çağrıları ve sürelerini inceleyin.
- **Metrikler için:** Veri kaynağını **Prometheus** seçin, `http_requests_total` gibi metrikleri aratarak sistem yükünü analiz edin.
