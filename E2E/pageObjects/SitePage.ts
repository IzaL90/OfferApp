import { Page, Locator } from "playwright"
import { expect } from '@playwright/test';
export class SitePage {
    public readonly page: Page;
    public readonly root: Locator;
    public readonly home: Locator;
    public readonly aperture: Locator;
    public readonly clickedHome: Locator;

    constructor(page: Page) {
        this.page = page;
        this.root = this.page.locator("//button[@data-name='bid-add-button']");
    }

    async isVisible(): Promise<boolean> {
        return this.root.isVisible();
    }

    async clickButton(): Promise<void> {
        await this.root.click();
    }

    async expectText(): Promise<void> {
        await expect(this.root).toHaveText("Add")
    }

}