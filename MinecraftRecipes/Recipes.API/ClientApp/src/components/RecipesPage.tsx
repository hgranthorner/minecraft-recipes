import React, {useEffect, useState} from 'react';
import axios from 'axios'
import {Recipe} from './Recipe';
import {useLocation} from 'react-router-dom';

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

export function RecipesPage(props: any) {
    const urlSearchParams = new URLSearchParams(useLocation().search);
    const queryFromURL = urlSearchParams.get('q')
    const [recipes, setRecipes] = useState<{ id: number, name: string }[]>([]);
    const [query, setQuery] = useState(queryFromURL || '');

    useEffect(() => {
        if (queryFromURL) {
            axios.get(`/api/Search?searchQuery=${queryFromURL}`).then(res => {
                setRecipes(res.data);
            })
        } else {
            setRecipes([]);
        }
    }, [queryFromURL]);

    useDebounce(() => {
        console.log(props);
        urlSearchParams.set('q', query);
        const location = {
            ...props.location,
            search: urlSearchParams.toString()
        };
        props.history.replace(location)
    }, [query], 1000)

    const handleSearchEvent = (e: { target: { value: string } }) => {
        setQuery(e.target.value);
    };
    return <>
        <div>Hello, RecipesPage!</div>
        <input type="text" onChange={handleSearchEvent} value={query || ''}/>
        <p>Results:</p>
        <ul>
            {recipes.map(item => <li key={item.id}><Recipe id={item.id}/></li>)}
        </ul>
    </>
}
