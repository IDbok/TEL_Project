﻿namespace TEL_ProjectBus.DAL.Interfaces;

public interface IRepository<T> where T : class
{
	IEnumerable<T> GetAll();
	T Get(int id);
	IEnumerable<T> Find(Func<T, Boolean> predicate);
	void Create(T item);
	void Update(T item);
	void Delete(int id);
}
