namespace TEL_ProjectBus.BLL.DTOs;

public record ProjectProfileDto
{
	public int ProjectId { get; init; }
	public string Name { get; init; }                     // Наименование
	public string Code { get; init; }                     // Код
	public string PreparedBy { get; init; }               // Подготовил
	public string Department { get; init; }               // Департамент
	public string Customer { get; init; }                 // Заказчик
	public string ResponsibleFullName { get; init; }      // ФИО ПМ ?
	public string ContactPhone { get; init; }             // Контактный телефон ПМ?
	public DateTime? DateCreated { get; init; }           // Дата создания
	public string GoalsAndRequirements { get; init; }     // Цели и требования
	public string CustomerNeeds { get; init; }            // Потребности заказчика
	public string SuccessCriteria { get; init; }          // Критерии успеха и измеримые цели
	public string HighLevelRisks { get; init; }           // Риски высокого уровня
	public string ScheduleAndBudget { get; init; }        // Краткое расписание и бюджет
	public string AssumptionsAndConstraints { get; init; }// Допущения и ограничения
	public string ConfidenceRequirements { get; init; }   // Требования к уверенности
	public string ProjectManager { get; init; }           // Назначение рук. проекта
	public string ChangedByUserId { get; init; }       // Идентификатор пользователя, который изменил паспорт
	public DateTime DateChanged { get; init; }            // Дата изменения паспорта
}
