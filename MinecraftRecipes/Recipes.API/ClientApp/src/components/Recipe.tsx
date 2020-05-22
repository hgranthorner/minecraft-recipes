import React, { useEffect, useState } from "react";
import axios from "axios";
import "./Recipe.css";
import { PatternGrid } from "./PatternGrid";

export function Recipe(props: { id: number }) {
  const { id } = props;
  const [recipe, setRecipe] = useState();
  useEffect(() => {
    axios.get(`/api/Recipes/${id}`).then(res => setRecipe(res.data));
  }, [id]);

  return (
    <div className="recipe">
      {recipe && (
        <>
          <ul>
            <li>Recipe ID: {id}</li>
            <li>Recipe Name: {recipe.recipeName}</li>
            <li>
              Image: <img src={`/images/${recipe.result.name}.png`} />
            </li>
            <li>
              Pattern:{" "}
              <PatternGrid
                pattern={recipe.pattern}
                patternInformation={recipe.patternInformation}
              />
            </li>
            <li>
              JSON:
              <details>
                <pre>{JSON.stringify(recipe, null, 2)}</pre>
              </details>
            </li>
          </ul>
        </>
      )}
    </div>
  );
}
