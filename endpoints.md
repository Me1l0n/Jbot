# Все эндпоинты journal.top-academy.ru

**Base URL:** `https://msapi.top-academy.ru/api/v2`  
**File Storage:** `https://fs.top-academy.ru/api/v1/files`

**Обязательные заголовки:**
```
Accept: application/json, text/plain, */*
Referer: https://journal.top-academy.ru/
Origin: https://journal.top-academy.ru
Authorization: Bearer <access_token>   (после авторизации)
```

> Источник: `app_bundle.js` — оригинальный бандл веб-приложения journal.top-academy.ru  
> Найдено: **155 эндпоинтов**

---

## 1. АВТОРИЗАЦИЯ (`/auth`) — 10 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 1 | POST | `/auth/login` | Авторизация студента |
| 2 | POST | `/auth/login-admin` | Авторизация администратора |
| 3 | POST | `/auth/login-as-child` | Авторизация как ребёнок (для родителей) |
| 4 | POST | `/auth/login-as-student` | Авторизация от имени студента |
| 5 | POST | `/auth/login-market-admin` | Авторизация администратора маркета |
| 6 | POST | `/auth/logout-as-student` | Выход из режима «от имени студента» |
| 7 | POST | `/auth/refresh` | Обновление access_token |
| 8 | POST | `/auth/reset-password` | Сброс пароля |
| 9 | POST | `/auth/file-token` | Получение токена для загрузки файлов |
| 10 | HEAD | `/` | Пинг для проверки доступности API/сессии |

---

## 2. НАСТРОЙКИ / ПРОФИЛЬ (`/settings`, `/profile`) — 14 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 11 | GET | `/settings/user-info` | Полная информация о пользователе |
| 12 | GET | `/settings/group-specs` | Список предметов группы |
| 13 | GET | `/settings/history-specs` | История предметов |
| 14 | GET | `/settings/public-forms` | Публичные формы обучения |
| 15 | GET | `/settings/public-specs` | Публичные специальности |
| 16 | GET | `/settings/admin-groups` | Группы (для администратора) |
| 17 | GET | `/settings/admin-group-students` | Студенты группы (для администратора) |
| 18 | POST | `/settings/change-current-group` | Сменить текущую группу |
| 19 | GET | `/profile/operations/settings` | Настройки профиля |
| 20 | POST | `/profile/operations/change-password` | Смена пароля |
| 21 | POST | `/profile/operations/change-personal-data` | Изменение персональных данных |
| 22 | GET | `/profile/statistic/student-achievements` | Достижения студента |
| 23 | GET | `/public/cities` | Список городов |
| 24 | GET | `/public/languages` | Список языков |

---

## 3. DASHBOARD — ГЛАВНАЯ (`/dashboard`) — 10 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 25 | GET | `/dashboard/progress/activity` | Лента активности |
| 26 | GET | `/dashboard/progress/attendance-statistic` | Статистика посещаемости |
| 27 | GET | `/dashboard/progress/academic-performance` | Академическая успеваемость |
| 28 | GET | `/dashboard/progress/leader-group` | Лидеры группы |
| 29 | GET | `/dashboard/progress/leader-group-points` | Лидеры группы (по очкам) |
| 30 | GET | `/dashboard/progress/leader-stream` | Лидеры потока |
| 31 | GET | `/dashboard/progress/leader-stream-points` | Лидеры потока (по очкам) |
| 32 | GET | `/dashboard/chart/progress` | Графики прогресса |
| 33 | GET | `/dashboard/chart/attendance` | Графики посещаемости |
| 34 | GET | `/dashboard/chart/average-progress` | График среднего прогресса |

---

## 4. РАСПИСАНИЕ (`/schedule`) — 4 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 35 | GET | `/schedule/operations/get-by-date` | Расписание на дату |
| 36 | GET | `/schedule/operations/get-month` | Расписание на месяц |
| 37 | GET | `/schedule/operations/get-by-date-range` | Расписание за период |
| 38 | GET | `/schedule/operations/month-events` | События за месяц |

---

## 5. ОЦЕНКИ / ПРОГРЕСС (`/progress`) — 5 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 39 | GET | `/progress/operations/student-visits` | Посещения и оценки |
| 40 | GET | `/progress/operations/student-exams` | Результаты экзаменов |
| 41 | POST | `/progress/operations/student-exams-file` | Файл экзаменов |
| 42 | GET | `/progress/operations/plan-url` | URL учебного плана |
| 43 | GET | `/progress/operations/school-quarterly-grades` | Четвертные оценки (школа) |

---

## 6. ДОМАШНИЕ ЗАДАНИЯ (`/homework`) — 8 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 44 | GET | `/homework/operations/list` | Список домашних заданий |
| 45 | POST | `/homework/operations/create` | Создание/отправка домашнего задания |
| 46 | POST | `/homework/operations/delete` | Удаление домашнего задания |
| 47 | POST | `/homework/operations/has-material` | Проверка наличия материала |
| 48 | GET | `/homework/evaluation/operations/get` | Получить оценку домашнего задания |
| 49 | GET | `/homework/evaluation/operations/get-tags` | Теги оценки домашнего задания |
| 50 | POST | `/homework/evaluation/operations/save` | Сохранить оценку домашнего задания |
| 51 | GET | `/homework/settings/group-history` | История домашних заданий группы |

---

## 7. СЧЁТЧИКИ (`/count`) — 4 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 52 | GET | `/count/page-counters` | Общие счётчики страниц |
| 53 | GET | `/count/homework` | Счётчики домашних заданий |
| 54 | GET | `/count/library` | Счётчики библиотеки |
| 55 | POST | `/count/set-view-materials` | Пометить материалы просмотренными |

---

## 8. НОВОСТИ (`/news`) — 4 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 56 | GET | `/news/operations/latest-news` | Последние новости |
| 57 | GET | `/news/operations/detail-news` | Детали новости |
| 58 | GET | `/news/operations/count-unread` | Количество непрочитанных новостей |
| 59 | POST | `/news/operations/set-view` | Пометить новость прочитанной |

---

## 9. БИБЛИОТЕКА / КВИЗЫ (`/library`) — 10 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 60 | GET | `/library/operations/list` | Список материалов (с параметрами) |
| 61 | GET | `/library/operations/list-all` | Все материалы |
| 62 | POST | `/library/operations/check-url` | Проверка URL материала |
| 63 | POST | `/library/operations/set-use-material` | Отметить использование материала |
| 64 | GET | `/library/operations/quizzes-academic-debt` | Академическая задолженность по квизам |
| 65 | POST | `/library/quiz/run` | Запуск квиза |
| 66 | POST | `/library/quiz/finish` | Завершение квиза |
| 67 | GET | `/library/quiz/opened-interview` | Открытое интервью (квиз) |
| 68 | POST | `/library/quiz/delay-interview` | Отложить интервью |
| 69 | GET | `/library/quiz/get-time-between-retries` | Время между повторными попытками |

---

## 10. СИГНАЛЫ (`/signal`) — 12 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 70 | GET | `/signal/operations/signals-list` | Список сигналов |
| 71 | GET | `/signal/operations/signals-comments` | Комментарии к сигналу |
| 72 | GET | `/signal/operations/count-unread` | Количество непрочитанных сигналов |
| 73 | GET | `/signal/operations/problems-list` | Список проблем |
| 74 | GET | `/signal/operations/get-reference-data` | Справочные данные |
| 75 | GET | `/signal/operations/get-reference-status` | Справочные статусы |
| 76 | POST | `/signal/operations/create` | Создать сигнал |
| 77 | POST | `/signal/operations/update` | Обновить сигнал |
| 78 | POST | `/signal/operations/delete` | Удалить сигнал |
| 79 | POST | `/signal/operations/approve` | Подтвердить сигнал |
| 80 | POST | `/signal/operations/set-view` | Пометить сигнал просмотренным |
| 81 | POST | `/signal/operations/set-to-work` | Взять сигнал в работу |

---

## 11. ПОРТФОЛИО (`/portfolio`) — 6 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 82 | GET | `/portfolio/operations/list` | Список работ портфолио |
| 83 | POST | `/portfolio/operations/create` | Создать работу |
| 84 | POST | `/portfolio/operations/delete` | Удалить работу |
| 85 | GET | `/portfolio/operations/history` | История портфолио |
| 86 | GET | `/portfolio/operations/design-specs` | Специальности дизайна |
| 87 | GET | `/portfolio/operations/design-teachers` | Преподаватели дизайна |

---

## 12. ОПЛАТА (`/payment`) — 9 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 88 | GET | `/payment/operations/index` | Информация об оплате |
| 89 | GET | `/payment/operations/history` | История оплат |
| 90 | GET | `/payment/operations/schedule` | График оплат |
| 91 | GET | `/payment/operations/check-cancellation` | Проверка расторжения договора |
| 92 | GET | `/payment/operations/download-requisites` | Скачать реквизиты |
| 93 | POST | `/payment/operations/contract` | Договор |
| 94 | POST | `/payment/operations/link` | Ссылка на оплату |
| 95 | POST | `/payment/epay/link` | Ссылка на электронную оплату |
| 96 | HEAD | `/payment/epay/link` | Проверка существования платежного шлюза Paybox/Epay |

---

## 13. ВАКАНСИИ (`/vacancy`) — 4 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 97 | GET | `/vacancy/operations/available-vacancies` | Доступные вакансии |
| 98 | GET | `/vacancy/operations/settings` | Настройки вакансий |
| 99 | GET | `/vacancy/operations/count-unread` | Количество непрочитанных вакансий |
| 100 | POST | `/vacancy/operations/set-view` | Пометить вакансию просмотренной |

---

## 14. КОНТАКТЫ / РАССЫЛКИ / SMS (`/contacts`) — 12 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 101 | GET | `/contacts/operations/index` | Список контактов |
| 102 | POST | `/contacts/operations/send-ceo` | Отправить сообщение руководству |
| 103 | POST | `/contacts/operations/send-teach` | Отправить сообщение преподавателю |
| 104 | GET | `/contacts/mailing/emails-list` | Список email-рассылок |
| 105 | POST | `/contacts/mailing/send-confirmation` | Отправить подтверждение email |
| 106 | GET | `/contacts/mailing/check-confirmation` | Проверить подтверждение email |
| 107 | POST | `/contacts/mailing/confirm` | Подтвердить email |
| 108 | POST | `/contacts/mailing/delay-confirmation` | Отложить подтверждение |
| 109 | POST | `/contacts/mailing/unsubscribe` | Отписаться от рассылки |
| 110 | GET | `/contacts/mailing/check-unsubscription-key` | Проверить ключ отписки |
| 111 | POST | `/contacts/sms/send-code` | Отправить SMS-код |
| 112 | POST | `/contacts/sms/verified-phone` | Подтвердить телефон |

---

## 15. ОБРАТНАЯ СВЯЗЬ (`/feedback`) — 6 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 113 | GET | `/feedback/social-review/get-review-list` | Список отзывов |
| 114 | POST | `/feedback/social-review/screen-review` | Скрыть/показать отзыв |
| 115 | GET | `/feedback/students/evaluate-academy-day` | Оценка дня в академии |
| 116 | POST | `/feedback/students/comment-academy-day` | Комментарий к дню |
| 117 | POST | `/feedback/students/evaluate-lesson` | Оценить занятие |
| 118 | GET | `/feedback/students/evaluate-lesson-list` | Список занятий для оценки |

---

## 16. РЕФЕРАЛЬНАЯ ПРОГРАММА (`/referral`) — 4 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 119 | GET | `/referral/operations/list` | Список рефералов |
| 120 | POST | `/referral/operations/create` | Создать реферал |
| 121 | GET | `/referral/operations/check-new-reward` | Проверить новую награду |
| 122 | POST | `/referral/operations/read-new-reward` | Прочитать новую награду |

---

## 17. МАРКЕТ — ПОКУПАТЕЛЬ (`/market/customer`) — 5 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 123 | GET | `/market/customer/product/list` | Список товаров |
| 124 | POST | `/market/customer/product/restore-cart-products-list` | Восстановить товары корзины |
| 125 | POST | `/market/customer/order/create` | Создать заказ |
| 126 | GET | `/market/customer/order/list` | Список заказов |
| 127 | GET | `/market/customer/order/info` | Информация о заказе |

---

## 18. МАРКЕТ — АДМИНИСТРАТОР (`/market/admin`) — 14 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 128 | GET | `/market/admin/product/list` | Список товаров |
| 129 | GET | `/market/admin/product/info` | Информация о товаре |
| 130 | POST | `/market/admin/product/create` | Создать товар |
| 131 | POST | `/market/admin/product/update` | Обновить товар |
| 132 | POST | `/market/admin/product/delete` | Удалить товар |
| 133 | POST | `/market/admin/product/delete-image` | Удалить изображение товара |
| 134 | GET | `/market/admin/order/list` | Список заказов |
| 135 | GET | `/market/admin/order/info` | Информация о заказе |
| 136 | POST | `/market/admin/order/set-status` | Изменить статус заказа |
| 137 | GET | `/market/admin/common/instruction-url` | URL инструкции |
| 138 | GET | `/market/admin/promo-settings/streams` | Потоки для промо |
| 139 | GET | `/market/admin/promo-settings/study-forms` | Формы обучения для промо |
| 140 | POST | `/market/admin/promo-settings/change-visibility-form` | Изменить видимость формы |
| 141 | POST | `/market/admin/promo-settings/change-visibility-stream` | Изменить видимость потока |

---

## 19. ДОКУМЕНТЫ (`/documents`) — 5 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 142 | POST | `/documents/save` | Сохранить документ |
| 143 | POST | `/documents/set-field-value` | Установить значение поля |
| 144 | POST | `/documents/delete-field-value` | Удалить значение поля |
| 145 | GET | `/documents/get-profile-fields?studentId=` | Получение полей профиля для документов |
| 146 | GET | `/documents/get?studentId=` | Получение документа студента |

---

## 20. ПИТАНИЕ (`/nutrition`) — 7 эндпоинтов

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 147 | GET | `/nutrition/parent/get-balance` | Баланс питания (родитель) |
| 148 | GET | `/nutrition/parent/get-list-transaction` | Список транзакций (родитель) |
| 149 | GET | `/nutrition/parent/get-order-menu` | Меню для заказа (родитель) |
| 150 | POST | `/nutrition/parent/order-food` | Заказать еду (родитель) |
| 151 | POST | `/nutrition/parent/cancellation-order` | Отменить заказ (родитель) |
| 152 | GET | `/nutrition/student/get-qr-code` | QR-код студента |
| 153 | POST | `/nutrition/parent/top-up-account` | Пополнение баланса питания |

---

## 21. ИНДИВИДУАЛЬНОЕ ОБУЧЕНИЕ (`/individual`) — 3 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 154 | GET | `/individual/operations/get-list-voucher` | Список ваучеров |
| 155 | GET | `/individual/operations/info-payments` | Информация об оплатах |
| 156 | POST | `/individual/operations/create-order` | Создать заказ |

---

## 22. ОТЗЫВЫ (`/reviews`) — 2 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 157 | GET | `/reviews/index/list` | Список отзывов |
| 158 | GET | `/reviews/index/instruction` | Инструкция по отзывам |

---

## 23. ИСТОРИИ (`/story`) — 1 эндпоинт

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 159 | GET | `/story/operations/get-stories` | Получить истории |

---

## 24. МАТЕРИАЛЫ (`/material`) — 1 эндпоинт

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 160 | GET | `/material/operations/get-material` | Получить материал |

---

## 25. ПУБЛИЧНЫЕ ДАННЫЕ (`/public`) — 3 эндпоинта

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 161 | GET | `/public/cities` | Список городов |
| 162 | GET | `/public/tags` | Список тегов |
| 163 | GET | `/public/translations` | Переводы интерфейса |

---

## 26. ЭКЗАМЕНЫ (`/dashboard/info`) — 1 эндпоинт

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 164 | GET | `/dashboard/info/future-exams` | Будущие экзамены |

---

## 27. ФАЙЛОВОЕ ХРАНИЛИЩЕ (отдельный сервер) — 1 эндпоинт

| # | Метод | Endpoint | Описание |
|---|-------|----------|----------|
| 165 | POST | `https://fs.top-academy.ru/api/v1/files` | Загрузка файлов |

---

## Сводка по разделам

| Раздел | Кол-во | GET | POST | ДР. |
|--------|--------|-----|------|-----|
| АВТОРИЗАЦИЯ | 10 | 0 | 9 | 1 |
| НАСТРОЙКИ / ПРОФИЛЬ (`/settings`, `/profile`) | 14 | 11 | 3 | 0 |
| DASHBOARD — ГЛАВНАЯ | 10 | 10 | 0 | 0 |
| РАСПИСАНИЕ | 4 | 4 | 0 | 0 |
| ОЦЕНКИ / ПРОГРЕСС | 5 | 4 | 1 | 0 |
| ДОМАШНИЕ ЗАДАНИЯ | 8 | 4 | 4 | 0 |
| СЧЁТЧИКИ | 4 | 3 | 1 | 0 |
| НОВОСТИ | 4 | 3 | 1 | 0 |
| БИБЛИОТЕКА / КВИЗЫ | 10 | 5 | 5 | 0 |
| СИГНАЛЫ | 12 | 6 | 6 | 0 |
| ПОРТФОЛИО | 6 | 4 | 2 | 0 |
| ОПЛАТА | 9 | 5 | 3 | 1 |
| ВАКАНСИИ | 4 | 3 | 1 | 0 |
| КОНТАКТЫ / РАССЫЛКИ / SMS | 12 | 4 | 8 | 0 |
| ОБРАТНАЯ СВЯЗЬ | 6 | 3 | 3 | 0 |
| РЕФЕРАЛЬНАЯ ПРОГРАММА | 4 | 2 | 2 | 0 |
| МАРКЕТ — ПОКУПАТЕЛЬ | 5 | 3 | 2 | 0 |
| МАРКЕТ — АДМИНИСТРАТОР | 14 | 7 | 7 | 0 |
| ДОКУМЕНТЫ | 5 | 2 | 3 | 0 |
| ПИТАНИЕ | 7 | 4 | 3 | 0 |
| ИНДИВИДУАЛЬНОЕ ОБУЧЕНИЕ | 3 | 2 | 1 | 0 |
| ОТЗЫВЫ | 2 | 2 | 0 | 0 |
| ИСТОРИИ | 1 | 1 | 0 | 0 |
| МАТЕРИАЛЫ | 1 | 1 | 0 | 0 |
| ПУБЛИЧНЫЕ ДАННЫЕ | 3 | 3 | 0 | 0 |
| ЭКЗАМЕНЫ | 1 | 1 | 0 | 0 |
| ФАЙЛОВОЕ ХРАНИЛИЩЕ (отдельный сервер) | 1 | 0 | 1 | 0 |
| **ИТОГО** | **165** | **97** | **66** | **2** |
