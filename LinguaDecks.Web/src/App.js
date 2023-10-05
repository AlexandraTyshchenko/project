import AppRouter from './routes/Router';
import { BrowserRouter } from 'react-router-dom';
function App() {
  return (
    <div className="App">      
      <BrowserRouter>
        <AppRouter />
      </BrowserRouter>
    </div>
  );
}
export default App;
