import { expect } from "@playwright/test";
import { Page, Locator } from "playwright";
import { FormComponent } from "../components/FormComponent";

export class EditPage {
    public readonly page: Page;
    public readonly root: Locator;
    public readonly submit: Locator
    public readonly cancel: Locator
    public readonly form:FormComponent

    constructor(page: Page) {
        this.page = page;
        this.root = this.page.locator("//div//main");
        this.submit = this.root.locator("//button[@data-name='bid-submit-button']")
        this.cancel = this.root.locator("//button[@data-name='bid-close-button']")
        this.form = new FormComponent(this.page.locator("//form"))
    }

    async isVisible(): Promise<boolean> {
        return this.root.isVisible();
    }

    public async expectFormVisible(): Promise<void> {
        expect(this.form.expectFormVisible())
        await expect(this.submit).toBeVisible()
        await expect(this.cancel).toBeVisible()
    }
}