using MelonLoader;
using BTD_Mod_Helper;
using PathsPlusPlus;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using JetBrains.Annotations;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System.Runtime.CompilerServices;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

using MonkeySubFourthPath;

[assembly: MelonInfo(typeof(MonkeySubFourthPath.MonkeySubFourthPath), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MonkeySubFourthPath;

public class MonkeySubFourthPath : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<MonkeySubFourthPath>("MonkeySubFourthPath loaded!");
    }
    public class FourthPath2 : PathPlusPlus
    {
        public override string Tower => TowerType.MonkeySub;
        public override int UpgradeCount => 5;

    }
    public class PiercingDarts : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 250;
        public override int Tier => 1;
        public override string Icon => VanillaSprites.FasterDartsUpgradeIcon;

        public override string Description => "Torpedos pierce through more bloons.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.pierce *= 2;

        }
    }
    public class TacticalTorpedos : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 550;
        public override int Tier => 2;
        public override string Icon => VanillaSprites.RazorSharpShotsUpgradeIcon;

        public override string Description => "Tactical Torpedo darts explode with more torpedo-darts.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", Game.instance.model.GetTowerFromId("MonkeySub-120").GetAttackModel().weapons[0].projectile.Duplicate(), new ArcEmissionModel("ArcEmissionModel_", 4, 0.0f, 360.0f, null, false), true, false, false));

        }
    }
    public class FishingRod : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 1500;
        public override int Tier => 3;
        public override string Icon => "fisherIcon";

        public override string Description => "Can fish up golden fish for some extra money you can pick up.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var fishProj = Game.instance.model.GetTowerFromId("BananaFarm-100").GetAttackModel().weapons[0].Duplicate();
            fishProj.rate = 30f;
            fishProj.projectile.ApplyDisplay<goldfishDisplay>();
            fishProj.RemoveBehavior<EmissionsPerRoundFilterModel>();
            fishProj.projectile.GetBehavior<CashModel>().maximum = 250f;
            fishProj.projectile.GetBehavior<CashModel>().minimum= 25f;
            fishProj.fireWithoutTarget = true;
            attackModel.AddWeapon(fishProj);

        }
    }
    public class goldfishDisplay : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "goldFish");
        }
    }
    public class BetterPull : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 6500;
        public override int Tier => 4;
        public override string Icon => "goldFish";

        public override string Description => "Can fish up faster. Fish are overall worth more. Can fish up much more materials, having random effects.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            towerModel.isGlobalRange = true;
            attackModel.weapons[1].projectile.GetBehavior<CashModel>().minimum = 200f;
            attackModel.weapons[1].projectile.GetBehavior<CashModel>().maximum = 400f;
            attackModel.weapons[1].rate *= 0.85f;

            var blades = Game.instance.model.GetTowerFromId("DartMonkey-205").GetAttackModel().weapons[0].Duplicate();
            blades.emission = new ArcEmissionModel("ArcEmissionModel_", 64, 0.0f, 360.0f, null, false);
            blades.rate = 20f;
            blades.fireWithoutTarget = true;
            attackModel.AddWeapon(blades);
            var adoraTracker = Game.instance.model.GetTowerFromId("BombShooter-032").GetAttackModel().weapons[0].Duplicate();
            adoraTracker.emission = new ArcEmissionModel("ArcEmissionModel_", 20, 0.0f, 50.0f, null, false);
            adoraTracker.rate = 28f;
            adoraTracker.fireWithoutTarget = true;
            adoraTracker.projectile.AddBehavior(Game.instance.model.GetTowerFromId("Adora 20").GetAttackModel().weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().Duplicate());
            adoraTracker.projectile.GetBehavior<AdoraTrackTargetModel>().Lifespan *= 2;
            attackModel.AddWeapon(adoraTracker);
        }
    }
    public class UltimateFisher : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 35000;
        public override int Tier => 5;
        public override string Icon => "fisherIcon2";

        public override string Description => "Really fast fishing, and can attract bigger, more useful fishes, that have many different effects.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[1].projectile.GetBehavior<CashModel>().minimum = 800f;
            attackModel.weapons[1].projectile.GetBehavior<CashModel>().maximum = 1200f;
            attackModel.weapons[1].rate *= 0.85f;
            foreach (var weaponModel in towerModel.GetWeapons())
            {
                weaponModel.rate *= 0.4f;
            }
            var blades = Game.instance.model.GetTowerFromId("DartMonkey-500").GetAttackModel().weapons[0].Duplicate();
            blades.emission = new ArcEmissionModel("ArcEmissionModel_", 32, 0.0f, 360.0f, null, false);
            blades.rate = 16f;
            blades.fireWithoutTarget = true;
            attackModel.AddWeapon(blades);
            var blades2 = Game.instance.model.GetTowerFromId("CaptainChurchill 20").GetAttackModel().weapons[0].Duplicate();
            blades2.emission = new ArcEmissionModel("ArcEmissionModel_", 64, 0.0f, 360.0f, null, false);
            blades2.rate = 8f;
            blades2.fireWithoutTarget = true;
            attackModel.AddWeapon(blades2);
            var blades3 = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetAttackModel().weapons[0].Duplicate();
            blades3.emission = new ArcEmissionModel("ArcEmissionModel_", 24, 0.0f, 360.0f, null, false);
            blades3.rate = 14f;
            blades3.fireWithoutTarget = true;
            attackModel.AddWeapon(blades3);
        }
    }
}