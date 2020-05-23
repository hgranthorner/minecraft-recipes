import React, { useEffect, useState } from "react";
import axios, { CancelTokenSource } from "axios";
import { Recipe } from "./Recipe";
import * as queryString from "querystring";
import { useHistory, useLocation } from "react-router-dom";

export function RecipesPage() {
  const location = useLocation();
  const history = useHistory();
  const [recipes, setRecipes] = useState<{ id: number; name: string }[]>([]);

  const search = location.search.substring(1);
  const { q: query = "" } = queryString.parse(search);

  useEffect(() => {
    let source: CancelTokenSource;
    if (query) {
      source = axios.CancelToken.source();

      axios
        .get(`/api/Search?searchQuery=${query}`, {
          cancelToken: source.token
        })
        .then(res => {
          setRecipes(res.data);
        })
        .catch(() => {});
    } else {
      setRecipes([]);
    }

    return () => {
      if (source) {
        source.cancel();
      }
    };
  }, [query]);

  const handleSearchEvent = (e: { target: { value: string } }) => {
    const value = e.target.value;
    location.search = queryString.stringify({ q: value });
    history.replace(location);
  };
  return (
    <>
      <div>Hello, RecipesPage!</div>
      <input type="text" onChange={handleSearchEvent} value={query} />
      <p>Results:</p>
      <ul>
        {recipes.map(
          (item, i) =>
            i <= 50 && (
              <li key={item.id}>
                <Recipe id={item.id} />
              </li>
            )
        )}
      </ul>
    </>
  );
}
