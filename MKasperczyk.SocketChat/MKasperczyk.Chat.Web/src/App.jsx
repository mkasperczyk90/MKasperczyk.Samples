import './App.css';
import Chat from './pages/Chat/Chat';
import Login from './pages/Login/Login'
import Register from './pages/Register/Register'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faPaperPlane, faSearch, faCircle } from '@fortawesome/free-solid-svg-icons'
import { BrowserRouter, Routes, Route } from 'react-router-dom'


library.add(faPaperPlane, faSearch, faCircle)

function App() {
  return (
    <div className="App container">
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<Chat />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
