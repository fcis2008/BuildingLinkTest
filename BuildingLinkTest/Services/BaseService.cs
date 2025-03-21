using AutoMapper;
using BuildingLinkTest.Repository;

namespace BuildingLinkTest.Services
{
    /// <summary>
    /// Base service class for managing CRUD operations.
    /// </summary>
    /// <typeparam name="TCreateDto">The type of the create DTO.</typeparam>
    /// <typeparam name="TDto">The type of the DTO.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class BaseService<TCreateDto, TDto, TEntity> : IDisposable, IBaseService<TCreateDto, TDto> where TEntity : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{TCreateDto, TDto, TEntity}"/> class.
        /// </summary>
        /// <param name="repository">The repository instance.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        /// <param name="createDto">The create DTO.</param>
        /// <returns>The ID of the created entity.</returns>
        public async Task<int> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The DTO of the retrieved entity.</returns>
        public async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        /// <summary>
        /// Retrieves all entities with pagination asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A list of DTOs of the retrieved entities.</returns>
        public async Task<IEnumerable<TDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var entities = await _repository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to update.</param>
        /// <param name="updateDto">The update DTO.</param>
        public async Task UpdateAsync(int id, TDto updateDto)
        {
            var entity = _mapper.Map<TEntity>(updateDto);
            var entityIdProperty = typeof(TEntity).GetProperty("Id");
            if (entityIdProperty != null)
            {
                entityIdProperty.SetValue(entity, id);
            }
            await _repository.UpdateAsync(entity);
        }

        /// <summary>
        /// Deletes an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Disposes the resources used by the service.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by the service.
        /// </summary>
        /// <param name="disposing">A value indicating whether the method is called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources here.
            }
            // Dispose unmanaged resources here.
        }
    }
}