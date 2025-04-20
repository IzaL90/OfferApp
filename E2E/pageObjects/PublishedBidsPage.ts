import { Page, Locator } from "playwright";
import { expect } from '@playwright/test';
import { DeleteComponent } from "../components/DeleteComponent";
import { PublishUnpublishComponent } from "../components/PublishUnpublish";


export class PublishedBidsPage {
    public readonly page: Page;
    public readonly root: Locator;
    public readonly rowName:Locator
    public readonly id: Locator;
    public readonly name: Locator;
    public readonly price: Locator;
    public readonly updated: Locator;
    public readonly action: Locator;

    constructor(page: Page) {
        this.page = page;
        this.root = this.page.locator("//table[contains(@class,'table')]");
        this.rowName= this.page.locator("//tr[contains(@class, 'table-primary')]//td").first();
        this.id = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Id']")
        this.name = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Name']")
        this.price = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Price']")
        this.updated = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Updated']")
        this.action = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Action']")
    }

    public async isVisible(): Promise<boolean> {
        return this.root.isVisible();
    }

    public async expectColumns(): Promise<void> {
        await expect(this.id).toBeVisible()
        await expect(this.name).toBeVisible()
        await expect(this.price).toBeVisible()
        await expect(this.updated).toBeVisible()
        await expect(this.action).toBeVisible()
    }

    public getRowLocator(name: string): Locator {
        return this.page.locator(`//table[contains(@class,"table")]//td[contains(text(), "${name}")]`);
    }
}

