import { Page, Locator } from "playwright"
import { expect } from '@playwright/test';
import { AddComponent } from "../components/AddComponent";
import { FormComponent } from "../components/FormComponent";
import { DeleteComponent } from "../components/DeleteComponent";

export class SitePage {
    public readonly page: Page;
    public readonly root: Locator;
    public readonly add: Locator
    public readonly modal: AddComponent
    public readonly form: FormComponent
    public readonly deleteModal: DeleteComponent

    constructor(page: Page) {
        this.page = page;
        this.root = this.page.locator("//main")
        this.add = this.root.locator("//button[@data-name='bid-add-button']");
        this.modal = new AddComponent(this.page, this.root.locator("//div[@class='modal-content']"))
        this.deleteModal = new DeleteComponent(this.root.locator("//div[@class='modal-content']"))
        this.form = new FormComponent(this.page)
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