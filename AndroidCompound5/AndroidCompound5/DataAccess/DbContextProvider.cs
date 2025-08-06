namespace AndroidCompound5.DataAccess
{
	public static class DbContextProvider
	{
		private static AppDBContex _dbContext;

		public static void Init(string dbPath)
		{
			_dbContext ??= new AppDBContex(dbPath);
		}

		public static AppDBContex Instance
		{
			get
			{
				if (_dbContext == null)
					throw new InvalidOperationException("DbContext is not initialized. Call Init() first.");
				return _dbContext;
			}
		}
	}
}
