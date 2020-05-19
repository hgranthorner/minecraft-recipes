import React, {useEffect, useState} from 'react';
import axios from 'axios'

function useDebounce(callback: (...args: any[]) => void, deps: unknown[], ms: number) {
    const [handle, setHandle] = useState<number | null>(null);
    useEffect(() => {

        if (handle) {
            clearTimeout(handle);
        }

        const timeoutHandle: unknown = setTimeout(callback, ms);
        setHandle(timeoutHandle as number);
    }, deps)
}

export function RecipesPage() {

    const [query, setQuery] = useState('');
    const [recipes, setRecipes] = useState<{ id: number, name: string }[]>([]);

    useDebounce(() => {
        if (query) {
            axios.get(`/api/Search?searchQuery=${query}`).then(res => {
                setRecipes(res.data);
            })
        } else {
            setRecipes([]);
        }
    }, [query], 1000)

    return <>
        <div>Hello, RecipesPage!</div>
        <input type="text" onChange={e => setQuery(e.target.value)}/>
        <p>Query: {query}</p>
        <p>Results:</p>
        <ul>
            {recipes.map(item => <li key={item.id}>{item.name}</li>)}
        </ul>
    </>
}
