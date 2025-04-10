public interface IDataSerializer<T>
{
	public T SerializeObject();
	public void DeserializeObject(T data);
}
