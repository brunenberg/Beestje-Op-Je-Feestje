using Microsoft.AspNetCore.Http;

namespace UnitTests {
    public class TestSession : ISession {
        Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();
        string ISession.Id => throw new NotImplementedException();
        bool ISession.IsAvailable => throw new NotImplementedException();
        IEnumerable<string> ISession.Keys => _sessionStorage.Keys;

        void ISession.Clear() {
            _sessionStorage.Clear();
        }

        Task ISession.CommitAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }

        Task ISession.LoadAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }

        void ISession.Remove(string key) {
            _sessionStorage.Remove(key);
        }

        void ISession.Set(string key, byte[] value) {
            _sessionStorage[key] = value;
        }

        bool ISession.TryGetValue(string key, out byte[] value) {
            return _sessionStorage.TryGetValue(key, out value);
        }
    }

}
