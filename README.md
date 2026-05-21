<p align="center">
  <img src="https://img.shields.io/badge/.NET_MAUI-net10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20Windows%20%7C%20macOS-D97757?style=for-the-badge" />
  <img src="https://img.shields.io/badge/API-journal.top--academy.ru-E8C872?style=for-the-badge" />
</p>

<h1 align="center">📓 Jbot</h1>

<p align="center">
  <b>Неофициальный клиент для журнала <a href="https://journal.top-academy.ru">Top Academy</a></b><br/>
  Мобильное/десктоп-приложение на .NET MAUI для работы с API студенческого журнала
</p>

<p align="center">
  <a href="#-возможности">Возможности</a> •
  <a href="#-установка">Установка</a> •
  <a href="#-конфигурация">Конфигурация</a> •
  <a href="#-структура-проекта">Структура</a> •
  <a href="#-api-эндпоинты">API эндпоинты</a>
</p>

---

## ✨ Возможности

| Вкладка | Описание |
|---------|----------|
| 🏠 **Главная** | Профиль, аватар, уровень, очки, посещаемость, график прогресса, лидеры группы, лента активности |
| 📅 **Расписание** | Расписание на сегодня / на месяц с карточками уроков, преподавателями и аудиториями |
| 📊 **Оценки** | Оценки по предметам, фильтрация по типу, статистика (средняя, макс.), гистограмма распределения |
| 📝 **Домашки** | Список домашних заданий со счётчиками, фильтрация по статусу (текущие / проверено / просрочено) |
| 🔧 **API Explorer** | Встроенный REST-клиент для отправки произвольных запросов к API журнала |

### Особенности

- 🌙 **Тёмная тема** — приятный тёмный интерфейс с акцентным цветом
- ⚡ **Параллельная загрузка** — все данные загружаются одновременно через `Task.WhenAll`
- 🔐 **Безопасное хранение** — учётные данные вынесены в `.env` файл и не попадают в репозиторий
- 📱 **Кроссплатформенность** — работает на Android, iOS, Windows и macOS

---

## 🚀 Установка

### Требования

| Компонент | Версия |
|-----------|--------|
| .NET SDK | 10.0+ |
| Visual Studio 2022 / Rider | Последняя версия |
| MAUI Workload | Установлен |

### 1. Клонируйте репозиторий

```bash
git clone https://github.com/your-username/Jbot.git
cd Jbot
```

### 2. Настройте конфигурацию

```bash
copy .env.example .env
```

Откройте `.env` в любом текстовом редакторе и заполните ваши данные:

```env
API_BASE_URL=https://msapi.top-academy.ru/api/v2/
APPLICATION_KEY=ваш_ключ_приложения
USERNAME=ваш_логин
PASSWORD=ваш_пароль
```

> [!IMPORTANT]
> `APPLICATION_KEY` можно получить из DevTools браузера при авторизации на [journal.top-academy.ru](https://journal.top-academy.ru). Откройте вкладку Network, войдите в аккаунт и найдите запрос `auth/login` — ключ будет в теле запроса.

### 3. Соберите и запустите

#### ⚡ Быстрый старт (bat-файлы)

```
build.bat    — Проверит .NET SDK, MAUI workload, создаст .env (если нет) и соберёт проект
run.bat      — Запустит собранное приложение (автоматически вызовет build, если нужно)
```

Просто дважды кликните по `build.bat`, а затем по `run.bat` — всё сделается автоматически.

> [!TIP]
> `build.bat` сам проверит, установлен ли .NET SDK и MAUI workload. Если чего-то нет — подскажет, что делать. Если `.env` файл отсутствует — создаст его из шаблона и попросит заполнить.

#### 🔧 Ручная сборка (для опытных)

**Установка MAUI workload:**
```bash
dotnet workload install maui
```

**Сборка и запуск (Windows):**
```bash
dotnet build Jbot/Jbot.csproj -f net10.0-windows10.0.19041.0 -c Release
dotnet run --project Jbot/Jbot.csproj -f net10.0-windows10.0.19041.0
```

**Android (эмулятор):**
```bash
dotnet build Jbot/Jbot.csproj -f net10.0-android
```

**Visual Studio:** откройте `Jbot/Jbot.csproj` и нажмите **F5**.

---

## ⚙ Конфигурация

Проект использует `.env` файл для хранения учётных данных. Файл `.env` находится в корне репозитория и **не коммитится в git**.

| Переменная | Описание | Обязательна |
|------------|----------|:-----------:|
| `API_BASE_URL` | Базовый URL API журнала | Нет (есть значение по умолчанию) |
| `APPLICATION_KEY` | Ключ приложения для авторизации | ✅ |
| `USERNAME` | Логин студента | ✅ |
| `PASSWORD` | Пароль студента | ✅ |

---

## 📁 Структура проекта

```
Jbot/
├── .env.example              # Шаблон конфигурации
├── .gitignore                # Правила Git
├── build.bat                 # 🔨 Сборка проекта (проверки + компиляция)
├── run.bat                   # 🚀 Запуск приложения
├── README.md                 # Документация
├── endpoints.md              # Полный каталог API (детальный)
│
└── Jbot/                     # .NET MAUI проект
    ├── Jbot.csproj           # Конфигурация проекта
    ├── MauiProgram.cs        # Точка входа, DI-контейнер
    ├── App.xaml(.cs)          # Application root
    ├── AppShell.xaml(.cs)     # TabBar навигация (5 вкладок)
    │
    ├── Services/
    │   ├── EnvConfig.cs       # Парсер .env файла
    │   └── ApiService.cs      # HTTP-клиент, авторизация, обёртки API
    │
    ├── Views/
    │   ├── DashboardPage      # Главная: профиль, статистика, лидеры
    │   ├── SchedulePage       # Расписание: сегодня / месяц
    │   ├── GradesPage         # Оценки: таблицы, фильтры, графики
    │   ├── HomeworkPage       # Домашки: список, счётчики, фильтры
    │   └── ExplorerPage       # API Explorer: произвольные запросы
    │
    ├── Resources/
    │   ├── Fonts/             # OpenSans
    │   ├── Styles/            # Colors.xaml, Styles.xaml
    │   └── Raw/               # Статические ресурсы
    │
    └── Platforms/             # Android, iOS, macOS, Windows
```

---

## 📡 API эндпоинты

> **Base URL:** `https://msapi.top-academy.ru/api/v2`

> [!NOTE]
> Все запросы (кроме авторизации и публичных) требуют заголовок `Authorization: Bearer <token>`.
> Также обязательны заголовки `Referer: https://journal.top-academy.ru/` и `Origin: https://journal.top-academy.ru`.

---

### 🔑 Авторизация (`/auth`) — 9

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| POST | `/auth/login` | Авторизация студента |
| POST | `/auth/login-admin` | Авторизация администратора |
| POST | `/auth/login-as-child` | Авторизация как ребёнок (для родителей) |
| POST | `/auth/login-as-student` | Авторизация от имени студента |
| POST | `/auth/login-market-admin` | Авторизация администратора маркета |
| POST | `/auth/logout-as-student` | Выход из режима «от имени студента» |
| POST | `/auth/refresh` | Обновление access_token |
| POST | `/auth/reset-password` | Сброс пароля |
| POST | `/auth/file-token` | Получение токена для загрузки файлов |

<details>
<summary>📋 Пример авторизации</summary>

```json
// POST /auth/login
// Body:
{
  "application_key": "your_key",
  "id_city": null,
  "username": "your_login",
  "password": "your_password"
}

// Response:
{
  "access_token": "eyJ...",
  "refresh_token": "def...",
  "expires_in_access": 86400,
  "user_type": "student",
  "city_data": { ... }
}
```

</details>

---

### 👤 Настройки / Профиль (`/settings`, `/profile`) — 12

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/settings/user-info` | Полная информация о пользователе |
| GET | `/settings/group-specs` | Список предметов группы |
| GET | `/settings/history-specs` | История предметов |
| GET | `/settings/public-forms` | Публичные формы обучения |
| GET | `/settings/public-specs` | Публичные специальности |
| GET | `/settings/admin-groups` | Группы (для администратора) |
| GET | `/settings/admin-group-students` | Студенты группы (для администратора) |
| POST | `/settings/change-current-group` | Сменить текущую группу |
| GET | `/profile/operations/settings` | Настройки профиля |
| POST | `/profile/operations/change-password` | Смена пароля |
| POST | `/profile/operations/change-personal-data` | Изменение персональных данных |
| GET | `/profile/statistic/student-achievements` | Достижения студента |

---

### 🏠 Dashboard (`/dashboard`) — 10

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/dashboard/progress/activity` | Лента активности |
| GET | `/dashboard/progress/attendance-statistic` | Статистика посещаемости |
| GET | `/dashboard/progress/academic-performance` | Академическая успеваемость |
| GET | `/dashboard/progress/leader-group` | Лидеры группы |
| GET | `/dashboard/progress/leader-group-points` | Лидеры группы (по очкам) |
| GET | `/dashboard/progress/leader-stream` | Лидеры потока |
| GET | `/dashboard/progress/leader-stream-points` | Лидеры потока (по очкам) |
| GET | `/dashboard/chart/progress` | Графики прогресса |
| GET | `/dashboard/chart/attendance` | Графики посещаемости |
| GET | `/dashboard/chart/average-progress` | График среднего прогресса |
| GET | `/dashboard/info/future-exams` | Будущие экзамены |

---

### 📅 Расписание (`/schedule`) — 4

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/schedule/operations/get-by-date` | Расписание на дату |
| GET | `/schedule/operations/get-month` | Расписание на месяц |
| GET | `/schedule/operations/get-by-date-range` | Расписание за период |
| GET | `/schedule/operations/month-events` | События за месяц |

---

### 📊 Оценки / Прогресс (`/progress`) — 5

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/progress/operations/student-visits` | Посещения и оценки |
| GET | `/progress/operations/student-exams` | Результаты экзаменов |
| POST | `/progress/operations/student-exams-file` | Файл экзаменов |
| GET | `/progress/operations/plan-url` | URL учебного плана |
| GET | `/progress/operations/school-quarterly-grades` | Четвертные оценки (школа) |

---

### 📝 Домашние задания (`/homework`) — 8

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/homework/operations/list` | Список домашних заданий |
| POST | `/homework/operations/create` | Создание/отправка ДЗ |
| POST | `/homework/operations/delete` | Удаление ДЗ |
| POST | `/homework/operations/has-material` | Проверка наличия материала |
| GET | `/homework/evaluation/operations/get` | Получить оценку ДЗ |
| GET | `/homework/evaluation/operations/get-tags` | Теги оценки ДЗ |
| POST | `/homework/evaluation/operations/save` | Сохранить оценку ДЗ |
| GET | `/homework/settings/group-history` | История ДЗ группы |

<details>
<summary>📋 Параметры списка домашних заданий</summary>

```
GET /homework/operations/list?page=1&status=0&type=0&group_id=10

Статусы (status):
  0 — текущие
  1 — проверено
  2 — просрочено
  3 — на доработку
  5 — удалено
```

</details>

---

### 🔢 Счётчики (`/count`) — 4

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/count/page-counters` | Общие счётчики страниц |
| GET | `/count/homework` | Счётчики домашних заданий |
| GET | `/count/library` | Счётчики библиотеки |
| POST | `/count/set-view-materials` | Пометить материалы просмотренными |

---

### 📰 Новости (`/news`) — 4

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/news/operations/latest-news` | Последние новости |
| GET | `/news/operations/detail-news` | Детали новости |
| GET | `/news/operations/count-unread` | Количество непрочитанных |
| POST | `/news/operations/set-view` | Пометить прочитанной |

---

### 📚 Библиотека / Квизы (`/library`) — 10

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/library/operations/list` | Список материалов |
| GET | `/library/operations/list-all` | Все материалы |
| POST | `/library/operations/check-url` | Проверка URL материала |
| POST | `/library/operations/set-use-material` | Отметить использование |
| GET | `/library/operations/quizzes-academic-debt` | Академическая задолженность |
| POST | `/library/quiz/run` | Запуск квиза |
| POST | `/library/quiz/finish` | Завершение квиза |
| GET | `/library/quiz/opened-interview` | Открытое интервью |
| POST | `/library/quiz/delay-interview` | Отложить интервью |
| GET | `/library/quiz/get-time-between-retries` | Время между попытками |

---

### 🚨 Сигналы (`/signal`) — 12

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/signal/operations/signals-list` | Список сигналов |
| GET | `/signal/operations/signals-comments` | Комментарии к сигналу |
| GET | `/signal/operations/count-unread` | Непрочитанные сигналы |
| GET | `/signal/operations/problems-list` | Список проблем |
| GET | `/signal/operations/get-reference-data` | Справочные данные |
| GET | `/signal/operations/get-reference-status` | Справочные статусы |
| POST | `/signal/operations/create` | Создать сигнал |
| POST | `/signal/operations/update` | Обновить сигнал |
| POST | `/signal/operations/delete` | Удалить сигнал |
| POST | `/signal/operations/approve` | Подтвердить сигнал |
| POST | `/signal/operations/set-view` | Пометить просмотренным |
| POST | `/signal/operations/set-to-work` | Взять в работу |

---

### 🎨 Портфолио (`/portfolio`) — 6

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/portfolio/operations/list` | Список работ |
| POST | `/portfolio/operations/create` | Создать работу |
| POST | `/portfolio/operations/delete` | Удалить работу |
| GET | `/portfolio/operations/history` | История |
| GET | `/portfolio/operations/design-specs` | Специальности дизайна |
| GET | `/portfolio/operations/design-teachers` | Преподаватели дизайна |

---

### 💳 Оплата (`/payment`) — 8

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/payment/operations/index` | Информация об оплате |
| GET | `/payment/operations/history` | История оплат |
| GET | `/payment/operations/schedule` | График оплат |
| GET | `/payment/operations/check-cancellation` | Проверка расторжения |
| GET | `/payment/operations/download-requisites` | Скачать реквизиты |
| POST | `/payment/operations/contract` | Договор |
| POST | `/payment/operations/link` | Ссылка на оплату |
| POST | `/payment/epay/link` | Электронная оплата |

---

### 💼 Вакансии (`/vacancy`) — 4

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/vacancy/operations/available-vacancies` | Доступные вакансии |
| GET | `/vacancy/operations/settings` | Настройки |
| GET | `/vacancy/operations/count-unread` | Непрочитанные |
| POST | `/vacancy/operations/set-view` | Пометить просмотренной |

---

### 📧 Контакты / SMS (`/contacts`) — 11

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/contacts/operations/index` | Список контактов |
| POST | `/contacts/operations/send-ceo` | Сообщение руководству |
| POST | `/contacts/operations/send-teach` | Сообщение преподавателю |
| GET | `/contacts/mailing/emails-list` | Список email-рассылок |
| POST | `/contacts/mailing/send-confirmation` | Подтверждение email |
| GET | `/contacts/mailing/check-confirmation` | Проверка подтверждения |
| POST | `/contacts/mailing/confirm` | Подтвердить email |
| POST | `/contacts/mailing/delay-confirmation` | Отложить подтверждение |
| POST | `/contacts/mailing/unsubscribe` | Отписаться |
| GET | `/contacts/mailing/check-unsubscription-key` | Проверить ключ отписки |
| POST | `/contacts/sms/send-code` | Отправить SMS-код |
| POST | `/contacts/sms/verified-phone` | Подтвердить телефон |

---

### 💬 Обратная связь (`/feedback`) — 6

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/feedback/social-review/get-review-list` | Список отзывов |
| POST | `/feedback/social-review/screen-review` | Скрыть/показать отзыв |
| GET | `/feedback/students/evaluate-academy-day` | Оценка дня |
| POST | `/feedback/students/comment-academy-day` | Комментарий к дню |
| POST | `/feedback/students/evaluate-lesson` | Оценить занятие |
| GET | `/feedback/students/evaluate-lesson-list` | Занятия для оценки |

---

### 🤝 Реферальная программа (`/referral`) — 4

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/referral/operations/list` | Список рефералов |
| POST | `/referral/operations/create` | Создать реферал |
| GET | `/referral/operations/check-new-reward` | Проверить награду |
| POST | `/referral/operations/read-new-reward` | Прочитать награду |

---

### 🛒 Маркет — покупатель (`/market/customer`) — 5

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/market/customer/product/list` | Список товаров |
| POST | `/market/customer/product/restore-cart-products-list` | Восстановить корзину |
| POST | `/market/customer/order/create` | Создать заказ |
| GET | `/market/customer/order/list` | Список заказов |
| GET | `/market/customer/order/info` | Информация о заказе |

---

### 🏪 Маркет — администратор (`/market/admin`) — 14

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/market/admin/product/list` | Список товаров |
| GET | `/market/admin/product/info` | Информация о товаре |
| POST | `/market/admin/product/create` | Создать товар |
| POST | `/market/admin/product/update` | Обновить товар |
| POST | `/market/admin/product/delete` | Удалить товар |
| POST | `/market/admin/product/delete-image` | Удалить изображение |
| GET | `/market/admin/order/list` | Список заказов |
| GET | `/market/admin/order/info` | Информация о заказе |
| POST | `/market/admin/order/set-status` | Изменить статус |
| GET | `/market/admin/common/instruction-url` | URL инструкции |
| GET | `/market/admin/promo-settings/streams` | Потоки для промо |
| GET | `/market/admin/promo-settings/study-forms` | Формы обучения |
| POST | `/market/admin/promo-settings/change-visibility-form` | Видимость формы |
| POST | `/market/admin/promo-settings/change-visibility-stream` | Видимость потока |

---

### 📄 Документы (`/documents`) — 3

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| POST | `/documents/save` | Сохранить документ |
| POST | `/documents/set-field-value` | Установить значение поля |
| POST | `/documents/delete-field-value` | Удалить значение поля |

---

### 🍽️ Питание (`/nutrition`) — 6

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/nutrition/parent/get-balance` | Баланс (родитель) |
| GET | `/nutrition/parent/get-list-transaction` | Транзакции (родитель) |
| GET | `/nutrition/parent/get-order-menu` | Меню для заказа |
| POST | `/nutrition/parent/order-food` | Заказать еду |
| POST | `/nutrition/parent/cancellation-order` | Отменить заказ |
| GET | `/nutrition/student/get-qr-code` | QR-код студента |

---

### 🎓 Индивидуальное обучение (`/individual`) — 3

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/individual/operations/get-list-voucher` | Список ваучеров |
| GET | `/individual/operations/info-payments` | Информация об оплатах |
| POST | `/individual/operations/create-order` | Создать заказ |

---

### ⭐ Отзывы (`/reviews`) — 2

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/reviews/index/list` | Список отзывов |
| GET | `/reviews/index/instruction` | Инструкция |

---

### 📖 Истории / Материалы / Публичное — 5

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| GET | `/story/operations/get-stories` | Получить истории |
| GET | `/material/operations/get-material` | Получить материал |
| GET | `/public/cities` | Список городов |
| GET | `/public/tags` | Список тегов |
| GET | `/public/translations` | Переводы интерфейса |

---

### 📁 Файловое хранилище (отдельный сервер)

| Метод | Endpoint | Описание |
|:-----:|----------|----------|
| POST | `https://fs.top-academy.ru/api/v1/files` | Загрузка файлов |

---

### 📊 Сводка

| | GET | POST | Всего |
|---|:---:|:----:|:-----:|
| **Эндпоинтов** | 93 | 67 | **160** |

---

## 🛡️ Дисклеймер

> Этот проект создан **исключительно в образовательных целях** для изучения API журнала Top Academy.
> Автор не несёт ответственности за использование данного ПО. Используйте на свой страх и риск.

---

## 📜 Лицензия

MIT License — свободное использование, модификация и распространение.
