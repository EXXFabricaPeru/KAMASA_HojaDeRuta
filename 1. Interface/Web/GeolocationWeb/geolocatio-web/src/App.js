import './App.css';
import './styles/main.css'
import {BrowserRouter, Switch, Route} from "react-router-dom";
import SectorLocalization from "./view/SectorLocalization";

function App() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/sector" render={props=> <SectorLocalization />} />
      </Switch>
    </BrowserRouter>
  );
}

export default App;
