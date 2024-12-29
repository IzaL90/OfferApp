import { Page, Locator } from "playwright";
import { expect } from '@playwright/test';
import { DeleteComponent } from "../components/DeleteComponent";


export class TablePage {
    public readonly page: Page;
    public readonly root: Locator;
    public readonly rowName:Locator
    public readonly id: Locator;
    public readonly name: Locator;
    public readonly created: Locator;
    public readonly published: Locator;
    public readonly firstPrice: Locator;
    public readonly lastPrice: Locator;
    public readonly action: Locator;
    public readonly editButton: Locator
    public readonly deleteButton:Locator
    public readonly delete:DeleteComponent

    constructor(page: Page) {
        this.page = page;
        this.root = this.page.locator("//table[contains(@class,'table')]");
        this.rowName= this.page.locator("//tr[contains(@class, 'table-primary')]//td").first()
        this.id = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Id']")
        this.name = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Name']")
        this.created = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Created']")
        this.published = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Published']")
        this.firstPrice = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='FirstPrice']")
        this.lastPrice = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='LastPrice']")
        this.action = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Action']")
        this.editButton = this.page.locator("//span[contains(@class,'oi-pencil')]")
        this.deleteButton = this.page.locator("//span[contains(@class,'oi-trash')]")
        this.delete = new DeleteComponent(this.page.locator("//div[@class='modal-content']"))
    }

    public async isVisible(): Promise<boolean> {
        return this.root.isVisible();
    }

    public async expectColumns(): Promise<void> {
        await expect(this.id).toBeVisible()
        await expect(this.name).toBeVisible()
        await expect(this.created).toBeVisible()
        await expect(this.published).toBeVisible()
        await expect(this.firstPrice).toBeVisible()
        await expect(this.lastPrice).toBeVisible()
        await expect(this.action).toBeVisible()
    }

    public async clickEdit(): Promise<void> {
        await this.editButton.click();
    }

    public async clickDelete(): Promise<void> {
        await this.deleteButton.click();
    }

}

