﻿Подготовка к разделению проекта на разные библиотеки согласно Clean Architecture.

// Будут выделены следующие разделы:
1. Core - базовые классы и интерфейсы, которые будут использоваться во всех библиотеках.
2. DAL - классы, которые будут работать с данными (например, репозитории).
3. BLL - классы, которые будут реализовывать бизнес-логику.
4. WebAPI - классы, которые будут отвечать за пользовательский интерфейс.

5. Tests - классы, которые будут использоваться для тестирования.
6. TestConsole - классы, которые будут использоваться для тестирования в консольном приложении.

// Зависимости между библиотеками:
- Core не зависит от других библиотек.
- DAL зависит от Core.
- BLL зависит от Core и DAL.
- WebAPI зависит от Core и BLL.

- Tests зависит от Core, DAL, BLL и WebAPI.
- TestConsole зависит от Core, DAL, BLL и WebAPI.

// В проекте WebAPI будут использоваться следующие библиотеки:
- MassTransit
- MassTransit.Extensions.DependencyInjection
- MassTransit.RabbitMQ
- FluentValidation
- FluentValidation.DependencyInjectionExtensions

// В проекте BLL будут использоваться следующие библиотеки:
- CompareNETObjects ??
- AutoMapper ??
- AutoMapper.Extensions.Microsoft.DependencyInjection ??

// В проекте DAL будут использоваться следующие библиотеки:
- Microsoft.EntityFrameworkCore


// В проекте Core будут использоваться следующие библиотеки:
...



// Ощищие правила:
- Все ответы WebAPI должны быть наследованы от BaseResponse. => имют поля IsSuccess и Message (тут можно хранить ошибку).
- Возвращающие данные в ответах WebAPI дожны быть nullable. При ошибке возвращаем null, IsSuccess = false, Message = "ошибка".
- Ответ накоманду Create должен возвращать Id созданного объекта.
