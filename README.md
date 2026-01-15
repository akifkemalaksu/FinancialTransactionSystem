# Financial Transaction System (Finansal Transfer Sistemi)

Bu proje, hesaplar arası para transferlerini güvenli, ölçeklenebilir ve dağıtık bir yapıda gerçekleştiren, mikroservis mimarisi ile geliştirilmiş bir finansal işlem sistemidir.

Proje, **Event-Driven Architecture** ve **CQRS** desenlerini kullanır. Servisler arası iletişim **Kafka** üzerinden asenkron olarak sağlanır ve veri tutarlılığı için **Outbox/Inbox** desenleri uygulanmıştır.

## ✅ Önkoşullar

Bu projeyi çalıştırmak için aşağıdaki bileşenlerin sisteminizde kurulu olması beklenir:

- **Docker** ve **Docker Compose**
- **.NET 10 SDK** (lokal geliştirme, migration vb. işlemler için)
- İsteğe bağlı: Visual Studio, Rider veya VS Code gibi bir IDE

## 🚀 Teknolojiler ve Mimari

Bu proje aşağıdaki modern teknolojileri ve desenleri barındırır:

*   **Platform:** .NET 10
*   **Architecture:** Microservices, Clean Architecture, CQRS
*   **Database:** PostgreSQL (Entity Framework Core)
*   **Caching:** Redis
*   **Messaging:** Apache Kafka
*   **Resilience:** Polly
*   **Containerization:** Docker & Docker Compose
*   **Telemetry:** OpenTelemetry
*   **Monitoring & Metrics:** Prometheus
*   **Log Management:** Loki
*   **Distributed Tracing:** Tempo
*   **Dashboard:** Grafana

## 🏗️ Servisler

Sistem aşağıdaki temel mikroservislerden oluşur:

1.  **AccountService:** Kullanıcı hesaplarını yönetir, bakiye güncellemelerini yapar.
2.  **FraudDetectionService:** Transfer işlemlerini şüpheli aktivitelere karşı denetler.
3.  **LedgerService:** Tüm finansal işlemlerin muhasebe kayıtlarını tutar.
4.  **NotificationService:** İşlem durumları hakkında bildirimleri yönetir.
5.  **TransactionService:** Transfer sürecini başlatan ve koordine eden servistir.

## 🔄 İş Akışı (Yüksek Seviye)

- İstemci, **TransactionService** üzerinden para transferi talebi oluşturur.
- TransactionService, isteği kalıcı hale getirir ve `transfer-created` event'ini Kafka'ya yazar.
- **FraudDetectionService**, bu event'i dinleyerek şüpheli işlem kontrolü yapar ve sonucu sisteme bildirir.
- İşlem onaylandığında ilgili hesap bakiyeleri **AccountService** tarafından güncellenir ve sonuca göre `transfer-completed` veya `transfer-failed` event'leri üretilir.
- **LedgerService**, bu event'leri dinleyerek her hareket için defter (ledger) kaydını oluşturur.
- **NotificationService**, başarılı veya başarısız işlemler için uygun bildirimleri üretir (e-posta/SMS vb. entegrasyonlar için genişletilebilir).

## 📂 Proje Yapısı (Özet)

- `Services/`
  - Her mikroservis için **API**, **Application**, **Domain**, **Infrastructure** katmanları bulunur.
  - Servisler: `AccountService`, `FraudDetectionService`, `LedgerService`, `NotificationService`, `TransactionService`
- `Common/`
  - `Messaging`: Kafka entegrasyonu, event kontratları ve producer/handler soyutlamaları.
  - `Messaging.Persistence`: Outbox/Inbox desenleri, Kafka consumer altyapısı ve arka plan işleyicileri.
  - `ServiceDefaults`: CQRS altyapısı, ortak middleware'ler, OpenTelemetry, rate limiting gibi servisler arası paylaşılan bileşenler.
- `Observability/`
  - OpenTelemetry kolektör konfigürasyonu, Prometheus, Loki, Tempo ve Grafana ayar dosyaları.
- `docker-compose.yml`
  - PostgreSQL, Redis, Kafka, OpenTelemetry stack'i ve tüm mikroservislerin orkestrasyonundan sorumludur.

## 🛠️ Kurulum ve Çalıştırma (Docker)

Projeyi ayağa kaldırmak için **Docker** ve **Docker Compose** gereklidir.

### Sistemi Başlatma

Aşağıdaki komut ile tüm servisleri, veritabanlarını, Kafka'yı ve Observability stack'ini derleyip arka planda başlatabilirsiniz:

```bash
docker compose up --build -d
```

Bu komut şunları yapar:
*   PostgreSQL, Redis ve Kafka konteynerlerini başlatır.
*   Gerekli Kafka topic'lerini (`transfer-created`, `transfer-completed`, `transfer-failed`) oluşturur.
*   Observability stack'ini (Prometheus, Loki, Tempo, Grafana, OTEL Collector) ayağa kaldırır.
*   Tüm mikroservisleri (.NET 10) build eder ve çalıştırır.

### Sistemi Durdurma

Tüm konteynerleri durdurmak ve silmek için:

```bash
docker compose down
```

## ⚙️ Konfigürasyon

- Her servis için yapılandırmalar `appsettings.json`, `appsettings.Development.json` ve `appsettings.Docker.json` dosyalarına ayrılmıştır.
- Docker ortamında:
  - PostgreSQL bağlantı bilgileri ve veritabanı adları ilgili servisin `appsettings.Docker.json` dosyasında tanımlıdır.
  - Kafka ayarları (örneğin `BootstrapServers`, `GroupId`) yine aynı dosyalarda yer alır.
  - OpenTelemetry ayarları `OTEL_EXPORTER_OTLP_ENDPOINT` ortam değişkeni ile yapılandırılır (varsayılan: `http://otel-collector:4317`).
- `docker-compose.yml` içinde tanımlı veritabanı kullanıcı adı/şifresi (`admin` / `admin123`) yalnızca lokal geliştirme/demonstrasyon amaçlıdır. Üretim ortamında mutlaka daha güçlü kimlik bilgileriyle değiştirilmelidir.

## 🔍 Servis Portları

Docker üzerinde çalışan servislerin dışarıya açılan portları:

### Mikroservisler
*   **Account Service:** 5001
*   **Fraud Detection Service:** 5002
*   **Ledger Service:** 5003
*   **Notification Service:** 5004
*   **Transaction Service:** 5005

### Altyapı Servisleri
*   **PostgreSQL:** 5432
*   **Redis:** 6379
*   **Kafka:** 9092

### Observability Stack
*   **Grafana:** 3000
*   **Otel Collector:** 4317 (gRPC), 4318 (HTTP), 8889 (Prometheus metrics)
*   **Prometheus:** 9090
*   **Loki:** 3100
*   **Tempo:** 3200

## 📈 Monitoring & Observability (Grafana)

Sistem ayağa kalktıktan sonra log, metrik ve trace takibi için **Grafana** kullanılmaktadır.

### 1. Grafana Erişimi

Sistem ayağa kalktığında Grafana otomatik olarak yapılandırılmış şekilde gelir (Veri kaynakları `datasources.yml` üzerinden otomatik tanımlanır).

Tarayıcınızdan aşağıdaki adrese giderek erişebilirsiniz:

- **URL:** `http://localhost:3000`

### 2. Verileri Görüntüleme (Explore)

Veri kaynakları (Prometheus, Loki, Tempo) hazır olduğu için doğrudan sorgulama yapabilirsiniz:

1. Sol menüdeki **Explore** (pusula simgesi) sekmesine tıklayın.
2. Sol üstteki açılır menüden veri kaynağını seçin:
   - **Loki:** Logları incelemek için (örn. `container_name` filtresi ile).
   - **Tempo:** Trace'leri (izleri) görüntülemek ve mikroservisler arası akışı takip etmek için.
   - **Prometheus:** Sistem metriklerini (CPU, Request Count vb.) sorgulamak için.

### 3. OpenTelemetry Yapılandırması

Projede tüm servisler **OpenTelemetry** ile enstrümante edilmiştir. Telemetri verileri şu şekilde akar:

```
Servisler → OTEL Collector → Prometheus (Metrics)
                           → Loki (Logs)
                           → Tempo (Traces)
```

- **OTEL Collector** endpoint: `http://otel-collector:4317` (gRPC) veya `http://otel-collector:4318` (HTTP)
- Konfigürasyon dosyası: `Observability/otel-collector-config.yml`

### 4. Örnek Loki Sorguları

Logları filtrelemek için Grafana Explore'da kullanabileceğiniz örnek sorgular:

```logql
# Belirli bir servisin loglarını görüntüle
{container_name="account-service"}

# Tüm servislerdeki hata loglarını filtrele
{container_name=~".*-service"} |= "error"

# Belirli bir TraceId ile logları ara
{container_name=~".*-service"} | json | TraceId="<trace-id-here>"
```

### 5. Log ile Trace İlişkilendirmesi

> **Not:** Loglar içerisindeki `trace_id` alanları otomatik olarak Tempo ile ilişkilendirilmiştir. Bir log satırındaki trace ID'ye tıklayarak doğrudan o işlemin tüm akışını (trace) görüntüleyebilirsiniz.

Bu özellik sayesinde:
- Bir hata logundan başlayarak tüm servisler arası akışı takip edebilirsiniz.
- Hangi servisin ne kadar süre harcadığını görebilirsiniz.
- Hata noktasını kolayca tespit edebilirsiniz.

### 6. Prometheus Metrikleri

Prometheus aşağıdaki metrik türlerini toplar:

- **HTTP Request Metrics:** İstek sayısı, süre, durum kodları
- **Runtime Metrics:** GC, thread count, memory kullanımı
- **Custom Business Metrics:** Transfer sayıları, başarı/başarısızlık oranları

Örnek PromQL sorguları:

```promql
# Servis başına HTTP istek sayısı
http_server_request_duration_seconds_count

# Son 5 dakikadaki hata oranı
rate(http_server_request_duration_seconds_count{http_response_status_code=~"5.."}[5m])
```

### 7. Observability Dosyaları

`Observability/` klasöründeki konfigürasyon dosyaları:

| Dosya | Açıklama |
|-------|----------|
| `otel-collector-config.yml` | OpenTelemetry Collector pipeline konfigürasyonu |
| `prometheus.yml` | Prometheus scrape hedefleri |
| `loki.yml` | Loki log aggregation ayarları |
| `tempo.yml` | Tempo distributed tracing ayarları |
| `datasources.yml` | Grafana veri kaynakları (otomatik provisioning) |

## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.
