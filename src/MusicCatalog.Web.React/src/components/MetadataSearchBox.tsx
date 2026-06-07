import {useState} from "react";
import {Combobox, Loader, Text, TextInput, useCombobox,} from "@mantine/core";

import {useDebouncedValue} from "../hooks/useDebouncedValue";
import {useAlbumMetadataByQuery} from "../hooks/useMetadata";
import {IconSearch} from "@tabler/icons-react";

type MetadataSearchBoxProps = {
    onAlbumSelected: (releaseGroupId: string) => void;
};

export function MetadataSearchBox({onAlbumSelected}: MetadataSearchBoxProps) {
    const combobox = useCombobox();

    const [searchText, setSearchText] = useState("");
    const debouncedSearchText = useDebouncedValue(searchText, 500);

    const {
        data: results = [],
        isLoading,
    } = useAlbumMetadataByQuery(debouncedSearchText);

    const options = results.map((result) => (
        <Combobox.Option
            value={result.releaseGroupId ?? ""}
            key={result.releaseGroupId}
        >
            <Text fw={500}>{result.title}</Text>
            <Text size="sm" c="dimmed">
                {result.artistName}
            </Text>
        </Combobox.Option>
    ));

    return (
        <Combobox
            store={combobox}
            onOptionSubmit={(value) => {
                onAlbumSelected(value);
                combobox.closeDropdown();
            }}
        >
            <Combobox.Target>
                <TextInput
                    w={500}
                    value={searchText}
                    placeholder="Search MusicBrainz..."
                    onFocus={() => combobox.openDropdown()}
                    onBlur={() => combobox.closeDropdown()}
                    onChange={(event) => {
                        setSearchText(event.currentTarget.value);
                        combobox.openDropdown();
                    }}
                    leftSection={<IconSearch size={16}/>}
                />
            </Combobox.Target>

            <Combobox.Dropdown>
                <Combobox.Options mah={400} style={{overflowY: "auto"}}>
                    {isLoading && (
                        <Combobox.Empty>
                            <Loader size="sm"/>
                        </Combobox.Empty>
                    )}

                    {!isLoading && options.length === 0 && (
                        <Combobox.Empty>No results found</Combobox.Empty>
                    )}

                    {!isLoading && options}
                </Combobox.Options>
            </Combobox.Dropdown>
        </Combobox>
    );
}
