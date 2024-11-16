import { Page, Locator } from "playwright"
import { expect } from '@playwright/test';
import { AddComponent } from "../components/AddComponent";

export class SitePage {
    public readonly page: Page;
    public readonly root: Locator;
    public readonly add: Locator
    public readonly modal: AddComponent

    constructor(page: Page) {
        this.page = page;
        this.root = this.page.locator("//main")
        this.add = this.root.locator("//button[@data-name='bid-add-button']");
        this.modal = new AddComponent(this.root.locator("//div[@class='modal-content']"))
    }

    async isVisible(): Promise<boolean> {
        return this.root.isVisible();
    }

    async clickButton(): Promise<void> {
        await this.add.click();
    }

    async expectText(): Promise<void> {
        await expect(this.add).toHaveText("Add")
    }

}