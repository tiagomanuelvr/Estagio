import { UmbControllerBase } from "@umbraco-cms/backoffice/class-api";

//const rootUnique = "ca4249ed-2b23-433b-b522-63cabe5500d1"; // GUID oficial da Raiz do Content

const staticItems = [
    {
        unique: "1",
        entityType: "my-tree-item",
        parent: { unique: "1fb5d9f7-fe84-40f2-ba63-e27c3b05d110", entityType: "my-tree-root" },
        name: "First Item",
        hasChildren: false,
        isFolder: false,
        icon: "icon-document",
    },
    {
        unique: "2",
        entityType: "my-tree-item2",
        parent: { unique: "1fb5d9f7-fe84-40f2-ba63-e27c3b05d110", entityType: "my-tree-root" },
        name: "Second Item",
        hasChildren: false,
        isFolder: false,
        icon: "icon-document",
    },
];

export class MyStaticTreeRepository extends UmbControllerBase {
    async requestTreeRoot() {
        return {
            data: {
                unique: "1fb5d9f7-fe84-40f2-ba63-e27c3b05d110",
                entityType: "my-tree-root",
                name: "My Static Tree",
                hasChildren: true,
                isFolder: true,
                //isContainer: true,
                icon: "icon-folder",
            },
        };
    }

    async requestTreeRootItems() {
        const items = staticItems.filter((item) => item.parent.unique === null);
        return { data: { items, total: items.length } };
    }

    async requestTreeItemsOf(args) {
        const items = staticItems.filter(
            (item) => item.parent.unique === args.parent.unique
        );
        return { data: { items, total: items.length } };
    }

    async requestTreeItemAncestors() {
        return { data: [] };
    }
}

export { MyStaticTreeRepository as api };