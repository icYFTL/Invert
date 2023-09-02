using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Invert.Commands;
public static class Constants
{
    public static List<string> DeprecatedArgs = new()
    {
        "cl_righthand",
        "+forward;+jump;-attack;-attack2", // +Jumpthrow with step
        "-jump;-forward", // -Jumpthrow with step
        "+jump;-attack;-attack2;-jump", // Classic jumpthrow
    };

    public static List<string> DeprecatedCommands = new()
    {
        "cfgver",
        "cl_allowdownload",
        "cl_allowupload",
        "cl_autowepswitch",
        "cl_cmdrate",
        "cl_debugrumble",
        "cl_detail_avoid_force",
        "cl_detail_avoid_radius",
        "cl_detail_avoid_recover_speed",
        "cl_detail_max_sway",
        "cl_downloadfilter",
        "cl_freezecampanel_position_dynamic",
        "cl_grass_mip_bias",
        "cl_hud_background_alpha",
        "cl_hud_bomb_under_radar",
        "cl_hud_healthammo_style",
        "cl_hud_playercount_pos",
        "cl_hud_playercount_showcount",
        "cl_idealpitchscale",
        "cl_minimal_rtt_shadows",
        "cl_observercrosshair",
        "cl_rumblescale",
        "cl_showpluginmessages2",
        "cl_spec_follow_grenade_key",
        "cl_spec_mode",
        "cl_viewmodel_shift_left_amt",
        "cl_viewmodel_shift_right_amt",
        "closeonbuy",
        "commentary_firstrun",
        "con_allownotify",
        "demo_index_max_other",
        "force_audio_english",
        "g15_update_msec",
        "hud_takesshots",
        "lookspring",
        "lookstrafe",
        "m_customaccel",
        "m_customaccel_exponent",
        "m_customaccel_max",
        "m_customaccel_scale",
        "m_forward",
        "m_mouseaccel1",
        "m_mouseaccel2",
        "m_mousespeed",
        "m_rawinput",
        "m_side",
        "mat_enable_uber_shaders",
        "mat_monitorgamma",
        "mat_monitorgamma_tv_enabled",
        "mat_powersavingsmode",
        "mat_queue_report",
        "mat_spewalloc",
        "mat_texture_list_content_path",
        "muzzleflash_light",
        "net_scale",
        "net_steamcnx_allowrelay",
        "snd_ducking_off",
        "snd_dzmusic_volume",
        "snd_hrtf_distance_behind",
        "snd_hrtf_voice_delay",
        "snd_hwcompat",
        "snd_mix_async",
        "snd_mix_async_onetime_reset",
        "snd_music_volume_onetime_reset_2",
        "snd_musicvolume_multiplier_inoverlay",
        "snd_pitchquality",
        "snd_surround_speakers",
        "ss_splitmode",
        "store_version",
        "suitvolume",
        "triple_monitor_mode",
        "trusted_launch_once",
        "viewmodel_recoil",
        "voice_caster_enable",
        "voice_caster_scale",
        "voice_enable",
        "voice_forcemicrecord",
        "voice_mixer_boost",
        "voice_mixer_mute",
        "voice_mixer_volume",
        "voice_positional",
        "voice_system_enable",
        "zoom_sensitivity_ratio_mouse",
        "r_dynamic",
        "r_eyegloss",
        "r_eyemove",
        "cl_disablefreezecam",
        "cl_disablehtmlmotd",
        "cl_freezecameffects_showholiday",
        "cl_showhelp",
        "cl_clearhinthistory",
        "dsp_enhance_stereo",
        "mat_queue_mode"
    };

    public static List<Regex> DeprecatedArgsRegexes = new()
    {
        new Regex("^net_graph.*", RegexOptions.Compiled)
    };

    public static List<Regex> DeprecatedCommandsRegexes = new()
    {

    };


    public static List<Tuple<string, string>> ReplaceableArgs = new()
    {
        new Tuple<string, string>("+moveleft", "+left"),
        new Tuple<string, string>("+moveright", "+right"),
        new Tuple<string, string>("+moveright", "+right"),
        new Tuple<string, string>("use weapon_hegrenade", "slot6"),
        new Tuple<string, string>("use weapon_flashbang", "slot7"),
        new Tuple<string, string>("use weapon_smokegrenade", "slot8"),
        new Tuple<string, string>("use weapon_decoy", "slot9"),
        //new Tuple<string, string>("use weapon_molotov;use weapon_incgrenade", "slot10"),
        //new Tuple<string, string>("use weapon_incgrenade;use weapon_molotov", "slot10"),
        new Tuple<string, string>("use weapon_incgrenade", "slot10"),
        new Tuple<string, string>("use weapon_molotov", "slot10"),
        new Tuple<string, string>("+speed", "+sprint")
    };

    public static List<Tuple<Regex, string>> ReplaceableArgsRegexes = new()
    {
        new Tuple<Regex, string>(new Regex("use\\s+?weapon_molotov\\s{0,}?;\\s{0,}?use\\s+?weapon_incgrenade", RegexOptions.Compiled), "slot10"),
        new Tuple<Regex, string>(new Regex("use\\s+?weapon_incgrenade\\s{0,}?;\\s{0,}?use\\s+?weapon_molotov", RegexOptions.Compiled), "slot10"),
        new Tuple<Regex, string>(new Regex("r_cleardecals\\s{0,}?;?", RegexOptions.Compiled), "")
    };

    public static List<Tuple<string, string>> ReplaceableCommands = new()
    {
        new Tuple<string, string>("net_graph", "cq_netgraph")
    };
}
