using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;

namespace ASP_LABS.API.Services.BookService
{
	public interface IBookService
	{
		/// <summary>
		/// Получение списка всех объектов
		/// </summary>
		/// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
		/// <param name="pageNo">номер страницы списка</param>
		/// <param name="pageSize">количество объектов на странице</param>
		/// <returns></returns>
		public Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
		/// <summary>
		/// Поиск объекта по Id
		/// </summary>
		/// <param name="id">Идентификатор объекта</param>
		/// <returns></returns>
		public Task<ResponseData<Book>> GetBookByIdAsync(int id);
		/// <summary>
		/// Обновление объекта
		/// </summary>
		/// <param name="id">Id изменяемомго объекта</param>
		/// <param name="book">объект с новыми параметрами</param>
		/// <returns></returns>
		public Task<ResponseData<Book>> UpdateBookAsync(int id, Book book);
		/// <summary>
		/// Удаление объекта
		/// </summary>
		/// <param name="id">Id удаляемомго объекта</param>
		/// <returns></returns>
		public Task<ResponseData<bool>> DeleteBookAsync(int id);
		/// <summary>
		/// Создание объекта
		/// </summary>
		/// <param name="book">Новый объект</param>
		/// <returns>Созданный объект</returns>
		public Task<ResponseData<Book>> CreateBookAsync(Book book);
		/// <summary>
		/// Сохранить файл изображения для объекта
		/// </summary>
		/// <param name="id">Id объекта</param>
		/// <param name="formFile">файл изображения</param>
		/// <returns>Url к файлу изображения</returns
		public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);




















	}
}
