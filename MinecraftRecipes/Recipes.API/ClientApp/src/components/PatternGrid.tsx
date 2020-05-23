import React from "react";

type PatternInformation = {
  character: string;
  itemName: string;
  itemId: number;
};

export function PatternGrid(props: {
  pattern: string;
  patternInformation: PatternInformation[];
}) {
  const rows = props.pattern.split("\n").map(row => {
    return row
      .split("")
      .map(patternChar =>
        props.patternInformation.find(x => x.character === patternChar)
      );
  });

  return (
    <table>
      <tbody>
        {rows.map((r, ri) => (
          <tr key={ri}>
            {r.map((ingredient, i) => (
              <td key={i} style={{ minWidth: 64, minHeight: 64 }}>
                {ingredient && (
                  <img
                    src={`/images/${ingredient.itemName}.png`}
                    alt={ingredient.itemName}
                  />
                )}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
}
