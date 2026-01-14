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
*   Gerekli Kafka topic'lerini (`transfer-created`, `transfer-completed`) oluşturur.
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
*   **PostgreSQL:** 5432
*   **Redis:** 6379
*   **Kafka:** 9092
