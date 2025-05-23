﻿namespace TEL_ProjectBus.WebAPI.Messages.Events;

public record BudgetItemCreatedEvent
{
	public Guid BudgetItemId { get; init; }
	public string BudgetName { get; init; }
	public DateTime CreatedAt { get; init; }
}
