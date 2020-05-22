import React from 'react';
import './App.css';
import {BrowserRouter, Link, Route, Switch} from 'react-router-dom';
import {RecipesPage} from './components/RecipesPage';

function App() {
    return (
        <BrowserRouter>
            <nav>
                <ul>
                    <li>
                        <Link to="/">Home</Link>
                    </li>
                    <li>
                        <Link to="/recipes">Recipes</Link>
                    </li>
                </ul>
            </nav>

            <Switch>
                <Route exact path="/">
                    <p>This is the home page</p>
                </Route>
                <Route path="/recipes" component={RecipesPage} />
            </Switch>
        </BrowserRouter>
    );
}

export default App;
