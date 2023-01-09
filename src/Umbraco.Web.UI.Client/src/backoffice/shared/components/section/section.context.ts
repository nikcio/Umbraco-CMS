import type { Entity, ManifestSection, ManifestSectionView, ManifestTree } from '@umbraco-cms/models';
import { UniqueBehaviorSubject } from 'src/core/observable-api/unique-behavior-subject';

export class UmbSectionContext {
	
	
	#manifest;
	public readonly manifest;

	// TODO: what is the best context to put this in?
	private _activeTree = new UniqueBehaviorSubject<ManifestTree | undefined>(undefined);
	public readonly activeTree = this._activeTree.asObservable();

	// TODO: what is the best context to put this in?
	private _activeTreeItem = new UniqueBehaviorSubject<Entity | undefined>(undefined);
	public readonly activeTreeItem = this._activeTreeItem.asObservable();

	// TODO: what is the best context to put this in?
	private _activeView = new UniqueBehaviorSubject<ManifestSectionView | undefined>(undefined);
	public readonly activeView = this._activeView.asObservable();

	constructor(sectionManifest: ManifestSection) {
		this.#manifest = new UniqueBehaviorSubject<ManifestSection>(sectionManifest);
		this.manifest = this.#manifest.asObservable();
	}


	public setManifest(data: ManifestSection) {
		this.#manifest.next({ ...data });
	}

	public getData() {
		return this.#manifest.getValue();
	}

	public setActiveTree(tree: ManifestTree) {
		this._activeTree.next(tree);
	}

	public setActiveTreeItem(item: Entity) {
		this._activeTreeItem.next(item);
	}

	public setActiveView(view: ManifestSectionView) {
		this._activeView.next(view);
	}
}
