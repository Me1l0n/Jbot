Journal endpoints:
https://msapi.top-academy.ru/api/v2 - общий адрес доступа к API

ВАЖНО: Требуются заголовки:
  Accept: application/json, text/plain, */*
  Referer: https://journal.top-academy.ru/
  Origin: https://journal.top-academy.ru

═══════════════════════════════════════════════════
  АВТОРИЗАЦИЯ
═══════════════════════════════════════════════════

1. POST /auth/login — Авторизация (Bearer метод)
   Body (JSON): { application_key, id_city, username, password }
   Ответ: { access_token, refresh_token, expires_in_access, user_type, city_data, user_role }

2. POST /auth/refresh — Обновить токен

═══════════════════════════════════════════════════
  ПРОФИЛЬ / НАСТРОЙКИ
═══════════════════════════════════════════════════

3.  GET /settings/user-info — Полная информация о пользователе
    → full_name, birthday, age, level, photo, gaming_points, groups, visibility...

4.  GET /settings/group-specs — Список предметов группы
    → [{id, name, short_name}]

5.  GET /settings/change-current-group — Сменить группу

6.  GET /profile/operations/settings — Настройки профиля

7.  GET /profile/statistic/student-achievements — Достижения
    → [{translate_key, is_active, achieve_points}]

═══════════════════════════════════════════════════
  DASHBOARD (ГЛАВНАЯ)
═══════════════════════════════════════════════════

8.  GET /dashboard/progress/activity — Лента активности
    → [{date, action, current_point, point_types_name, achievements_name}]

9.  GET /dashboard/progress/attendance-statistic — Статистика посещаемости
    → {diffMonth, diffWeek, statMonth, statWeek, statTotal}

10. GET /dashboard/chart/progress — Графики прогресса
    → [{chart_type, chart_models: [{date, points}]}]

11. GET /dashboard/progress/leader-group — Лидеры группы
    → [{full_name, photo_path, position, amount}]

12. GET /dashboard/progress/leader-stream — Лидеры потока
    → [{full_name, photo_path, position, amount}]

13. GET /dashboard/info/future-exams — Будущие экзамены

═══════════════════════════════════════════════════
  ДОМАШКИ
═══════════════════════════════════════════════════

14. GET /homework/operations/list?page=X1&status=X2&type=X3&group_id=X4
    → [{id, theme, fio_teach, name_spec, completion_time, creation_time, overdue_time, status, mark}]

15. GET /count/homework — Счётчики домашек
    → [{counter_type, counter}] (0=текущие, 1=проверено, 2=просрочено, 3=доработка, 4=всего, 5=выполнено)

═══════════════════════════════════════════════════
  РАСПИСАНИЕ
═══════════════════════════════════════════════════

16. GET /schedule/operations/get-by-date — Расписание на сегодня
    → [{date, lesson, started_at, finished_at, teacher_name, subject_name, room_name}]

17. GET /schedule/operations/get-month — Расписание на месяц

18. GET /schedule/operations/get-by-date-range — Расписание за период

═══════════════════════════════════════════════════
  ОЦЕНКИ / ПРОГРЕСС
═══════════════════════════════════════════════════

19. GET /progress/operations/student-visits — Посещения и оценки
    → [{date_visit, lesson_number, teacher_name, spec_name, lesson_theme, class_work_mark, home_work_mark, lab_work_mark}]

20. GET /progress/operations/student-exams — Экзамены
    → [{teacher, mark, mark_type, date, spec}]

═══════════════════════════════════════════════════
  НОВОСТИ
═══════════════════════════════════════════════════

21. GET /news/operations/latest-news — Последние новости
    → [{id_bbs, theme, time, viewed}]

22. GET /news/operations/count-unread — Кол-во непрочитанных
    → {counter_type, counter}

═══════════════════════════════════════════════════
  СЧЁТЧИКИ
═══════════════════════════════════════════════════

23. GET /count/page-counters — Общие счётчики страниц
    → [{counter_type, counter}]

24. GET /count/library — Счётчики библиотеки
    → [{material_type_id, materials_count, new_count, recommended_count}]

═══════════════════════════════════════════════════
  ДРУГИЕ
═══════════════════════════════════════════════════

25. GET /library/operations/list — Библиотека материалов (нужны параметры)
26. GET /signal/operations/signals-list — Сигналы
27. GET /vacancy/operations/available-vacancies — Вакансии
28. GET /portfolio/operations/list — Портфолио
29. GET /payment/operations/index — Оплата
30. GET /payment/operations/history — История оплат
31. GET /payment/operations/schedule — График оплат
