import { useState } from 'react';
import AdminPage from './AdminPage';
import HomePage from './HomePage';

function App() {
  const [route, setRoute] = useState(window.location.pathname.includes('/admin') ? 'admin' : 'home');

  const goHome = () => {
    window.history.pushState({}, '', '/');
    setRoute('home');
  };

  const goAdmin = () => {
    window.history.pushState({}, '', '/admin');
    setRoute('admin');
  };

  return (
    <div>
      <nav className="page-shell" style={{ paddingBottom: 0 }}>
        <div className="row" style={{ justifyContent: 'flex-end' }}>
          <button type="button" onClick={goHome}>Home</button>
          <button type="button" onClick={goAdmin}>Admin</button>
        </div>
      </nav>
      {route === 'admin' ? <AdminPage /> : <HomePage />}
    </div>
  );
}

export default App;
