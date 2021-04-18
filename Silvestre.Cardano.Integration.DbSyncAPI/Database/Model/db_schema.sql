--
-- PostgreSQL database dump
--

-- Dumped from database version 11.9
-- Dumped by pg_dump version 11.9

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: addr29type; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.addr29type AS bytea
	CONSTRAINT addr29type_check CHECK ((octet_length(VALUE) = 29));


ALTER DOMAIN public.addr29type OWNER TO nixbld;

--
-- Name: asset32type; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.asset32type AS bytea
	CONSTRAINT asset32type_check CHECK ((octet_length(VALUE) <= 32));


ALTER DOMAIN public.asset32type OWNER TO nixbld;

--
-- Name: hash28type; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.hash28type AS bytea
	CONSTRAINT hash28type_check CHECK ((octet_length(VALUE) = 28));


ALTER DOMAIN public.hash28type OWNER TO nixbld;

--
-- Name: hash32type; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.hash32type AS bytea
	CONSTRAINT hash32type_check CHECK ((octet_length(VALUE) = 32));


ALTER DOMAIN public.hash32type OWNER TO nixbld;

--
-- Name: int65type; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.int65type AS numeric(20,0)
	CONSTRAINT int65type_check CHECK (((VALUE >= '-18446744073709551615'::numeric) AND (VALUE <= '18446744073709551615'::numeric)));


ALTER DOMAIN public.int65type OWNER TO nixbld;

--
-- Name: lovelace; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.lovelace AS numeric(20,0)
	CONSTRAINT lovelace_check CHECK (((VALUE >= (0)::numeric) AND (VALUE <= '18446744073709551615'::numeric)));


ALTER DOMAIN public.lovelace OWNER TO nixbld;

--
-- Name: word128type; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.word128type AS numeric(39,0)
	CONSTRAINT word128type_check CHECK (((VALUE >= (0)::numeric) AND (VALUE <= '340282366920938463463374607431768211455'::numeric)));


ALTER DOMAIN public.word128type OWNER TO nixbld;

--
-- Name: outsum; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.outsum AS public.word128type;


ALTER DOMAIN public.outsum OWNER TO nixbld;

--
-- Name: rewardtype; Type: TYPE; Schema: public; Owner: nixbld
--

CREATE TYPE public.rewardtype AS ENUM (
    'leader',
    'member'
);


ALTER TYPE public.rewardtype OWNER TO nixbld;

--
-- Name: txindex; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.txindex AS smallint
	CONSTRAINT txindex_check CHECK ((VALUE >= 0));


ALTER DOMAIN public.txindex OWNER TO nixbld;

--
-- Name: uinteger; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.uinteger AS integer
	CONSTRAINT uinteger_check CHECK ((VALUE >= 0));


ALTER DOMAIN public.uinteger OWNER TO nixbld;

--
-- Name: word64type; Type: DOMAIN; Schema: public; Owner: nixbld
--

CREATE DOMAIN public.word64type AS numeric(20,0)
	CONSTRAINT word64type_check CHECK (((VALUE >= (0)::numeric) AND (VALUE <= '18446744073709551615'::numeric)));


ALTER DOMAIN public.word64type OWNER TO nixbld;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: ada_pots; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.ada_pots (
    id bigint NOT NULL,
    slot_no public.uinteger NOT NULL,
    epoch_no public.uinteger NOT NULL,
    treasury public.lovelace NOT NULL,
    reserves public.lovelace NOT NULL,
    rewards public.lovelace NOT NULL,
    utxo public.lovelace NOT NULL,
    deposits public.lovelace NOT NULL,
    fees public.lovelace NOT NULL,
    block_id bigint NOT NULL
);


ALTER TABLE public.ada_pots OWNER TO nixbld;

--
-- Name: ada_pots_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.ada_pots_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ada_pots_id_seq OWNER TO nixbld;

--
-- Name: ada_pots_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.ada_pots_id_seq OWNED BY public.ada_pots.id;


--
-- Name: block; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.block (
    id bigint NOT NULL,
    hash public.hash32type NOT NULL,
    epoch_no public.uinteger,
    slot_no public.uinteger,
    epoch_slot_no public.uinteger,
    block_no public.uinteger,
    previous_id bigint,
    merkle_root public.hash32type,
    slot_leader_id bigint NOT NULL,
    size public.uinteger NOT NULL,
    "time" timestamp without time zone NOT NULL,
    tx_count bigint NOT NULL,
    proto_major public.uinteger NOT NULL,
    proto_minor public.uinteger NOT NULL,
    vrf_key character varying,
    op_cert public.hash32type
);


ALTER TABLE public.block OWNER TO nixbld;

--
-- Name: block_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.block_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.block_id_seq OWNER TO nixbld;

--
-- Name: block_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.block_id_seq OWNED BY public.block.id;


--
-- Name: delegation; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.delegation (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    cert_index integer NOT NULL,
    pool_hash_id bigint NOT NULL,
    active_epoch_no bigint NOT NULL,
    tx_id bigint NOT NULL,
    slot_no public.uinteger NOT NULL
);


ALTER TABLE public.delegation OWNER TO nixbld;

--
-- Name: delegation_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.delegation_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.delegation_id_seq OWNER TO nixbld;

--
-- Name: delegation_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.delegation_id_seq OWNED BY public.delegation.id;


--
-- Name: epoch; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.epoch (
    id bigint NOT NULL,
    out_sum public.word128type NOT NULL,
    fees public.lovelace NOT NULL,
    tx_count public.uinteger NOT NULL,
    blk_count public.uinteger NOT NULL,
    no public.uinteger NOT NULL,
    start_time timestamp without time zone NOT NULL,
    end_time timestamp without time zone NOT NULL
);


ALTER TABLE public.epoch OWNER TO nixbld;

--
-- Name: epoch_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.epoch_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.epoch_id_seq OWNER TO nixbld;

--
-- Name: epoch_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.epoch_id_seq OWNED BY public.epoch.id;


--
-- Name: epoch_param; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.epoch_param (
    id bigint NOT NULL,
    epoch_no public.uinteger NOT NULL,
    min_fee_a public.uinteger NOT NULL,
    min_fee_b public.uinteger NOT NULL,
    max_block_size public.uinteger NOT NULL,
    max_tx_size public.uinteger NOT NULL,
    max_bh_size public.uinteger NOT NULL,
    key_deposit public.lovelace NOT NULL,
    pool_deposit public.lovelace NOT NULL,
    max_epoch public.uinteger NOT NULL,
    optimal_pool_count public.uinteger NOT NULL,
    influence double precision NOT NULL,
    monetary_expand_rate double precision NOT NULL,
    treasury_growth_rate double precision NOT NULL,
    decentralisation double precision NOT NULL,
    entropy public.hash32type,
    protocol_major public.uinteger NOT NULL,
    protocol_minor public.uinteger NOT NULL,
    min_utxo_value public.lovelace NOT NULL,
    min_pool_cost public.lovelace NOT NULL,
    nonce public.hash32type,
    block_id bigint NOT NULL
);


ALTER TABLE public.epoch_param OWNER TO nixbld;

--
-- Name: epoch_param_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.epoch_param_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.epoch_param_id_seq OWNER TO nixbld;

--
-- Name: epoch_param_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.epoch_param_id_seq OWNED BY public.epoch_param.id;


--
-- Name: epoch_stake; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.epoch_stake (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    pool_id bigint NOT NULL,
    amount public.lovelace NOT NULL,
    epoch_no bigint NOT NULL,
    block_id bigint NOT NULL
);


ALTER TABLE public.epoch_stake OWNER TO nixbld;

--
-- Name: epoch_stake_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.epoch_stake_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.epoch_stake_id_seq OWNER TO nixbld;

--
-- Name: epoch_stake_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.epoch_stake_id_seq OWNED BY public.epoch_stake.id;


--
-- Name: ma_tx_mint; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.ma_tx_mint (
    id bigint NOT NULL,
    policy public.hash28type NOT NULL,
    name public.asset32type NOT NULL,
    quantity public.int65type NOT NULL,
    tx_id bigint NOT NULL
);


ALTER TABLE public.ma_tx_mint OWNER TO nixbld;

--
-- Name: ma_tx_mint_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.ma_tx_mint_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ma_tx_mint_id_seq OWNER TO nixbld;

--
-- Name: ma_tx_mint_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.ma_tx_mint_id_seq OWNED BY public.ma_tx_mint.id;


--
-- Name: ma_tx_out; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.ma_tx_out (
    id bigint NOT NULL,
    policy public.hash28type NOT NULL,
    name public.asset32type NOT NULL,
    quantity public.word64type NOT NULL,
    tx_out_id bigint NOT NULL
);


ALTER TABLE public.ma_tx_out OWNER TO nixbld;

--
-- Name: ma_tx_out_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.ma_tx_out_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ma_tx_out_id_seq OWNER TO nixbld;

--
-- Name: ma_tx_out_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.ma_tx_out_id_seq OWNED BY public.ma_tx_out.id;


--
-- Name: meta; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.meta (
    id bigint NOT NULL,
    start_time timestamp without time zone NOT NULL,
    network_name character varying NOT NULL
);


ALTER TABLE public.meta OWNER TO nixbld;

--
-- Name: meta_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.meta_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.meta_id_seq OWNER TO nixbld;

--
-- Name: meta_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.meta_id_seq OWNED BY public.meta.id;


--
-- Name: orphaned_reward; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.orphaned_reward (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    amount public.lovelace NOT NULL,
    epoch_no bigint NOT NULL,
    pool_id bigint NOT NULL,
    block_id bigint NOT NULL,
    type public.rewardtype NOT NULL
);


ALTER TABLE public.orphaned_reward OWNER TO nixbld;

--
-- Name: orphaned_reward_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.orphaned_reward_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.orphaned_reward_id_seq OWNER TO nixbld;

--
-- Name: orphaned_reward_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.orphaned_reward_id_seq OWNED BY public.orphaned_reward.id;


--
-- Name: param_proposal; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.param_proposal (
    id bigint NOT NULL,
    epoch_no public.uinteger NOT NULL,
    key public.hash28type NOT NULL,
    min_fee_a public.uinteger,
    min_fee_b public.uinteger,
    max_block_size public.uinteger,
    max_tx_size public.uinteger,
    max_bh_size public.uinteger,
    key_deposit public.lovelace,
    pool_deposit public.lovelace,
    max_epoch public.uinteger,
    optimal_pool_count public.uinteger,
    influence double precision,
    monetary_expand_rate double precision,
    treasury_growth_rate double precision,
    decentralisation double precision,
    entropy public.hash32type,
    protocol_major public.uinteger,
    protocol_minor public.uinteger,
    min_utxo_value public.lovelace,
    min_pool_cost public.lovelace,
    registered_tx_id bigint NOT NULL
);


ALTER TABLE public.param_proposal OWNER TO nixbld;

--
-- Name: param_proposal_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.param_proposal_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.param_proposal_id_seq OWNER TO nixbld;

--
-- Name: param_proposal_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.param_proposal_id_seq OWNED BY public.param_proposal.id;


--
-- Name: pool_hash; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.pool_hash (
    id bigint NOT NULL,
    hash_raw public.hash28type NOT NULL,
    view character varying NOT NULL
);


ALTER TABLE public.pool_hash OWNER TO nixbld;

--
-- Name: pool_hash_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.pool_hash_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pool_hash_id_seq OWNER TO nixbld;

--
-- Name: pool_hash_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.pool_hash_id_seq OWNED BY public.pool_hash.id;


--
-- Name: pool_meta_data; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.pool_meta_data (
    id bigint NOT NULL,
    url character varying NOT NULL,
    hash public.hash32type NOT NULL,
    registered_tx_id bigint NOT NULL
);


ALTER TABLE public.pool_meta_data OWNER TO nixbld;

--
-- Name: pool_meta_data_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.pool_meta_data_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pool_meta_data_id_seq OWNER TO nixbld;

--
-- Name: pool_meta_data_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.pool_meta_data_id_seq OWNED BY public.pool_meta_data.id;


--
-- Name: pool_owner; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.pool_owner (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    pool_hash_id bigint NOT NULL,
    registered_tx_id bigint NOT NULL
);


ALTER TABLE public.pool_owner OWNER TO nixbld;

--
-- Name: pool_owner_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.pool_owner_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pool_owner_id_seq OWNER TO nixbld;

--
-- Name: pool_owner_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.pool_owner_id_seq OWNED BY public.pool_owner.id;


--
-- Name: pool_relay; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.pool_relay (
    id bigint NOT NULL,
    update_id bigint NOT NULL,
    ipv4 character varying,
    ipv6 character varying,
    dns_name character varying,
    dns_srv_name character varying,
    port integer
);


ALTER TABLE public.pool_relay OWNER TO nixbld;

--
-- Name: pool_relay_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.pool_relay_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pool_relay_id_seq OWNER TO nixbld;

--
-- Name: pool_relay_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.pool_relay_id_seq OWNED BY public.pool_relay.id;


--
-- Name: pool_retire; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.pool_retire (
    id bigint NOT NULL,
    hash_id bigint NOT NULL,
    cert_index integer NOT NULL,
    announced_tx_id bigint NOT NULL,
    retiring_epoch public.uinteger NOT NULL
);


ALTER TABLE public.pool_retire OWNER TO nixbld;

--
-- Name: pool_retire_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.pool_retire_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pool_retire_id_seq OWNER TO nixbld;

--
-- Name: pool_retire_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.pool_retire_id_seq OWNED BY public.pool_retire.id;


--
-- Name: pool_update; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.pool_update (
    id bigint NOT NULL,
    hash_id bigint NOT NULL,
    cert_index integer NOT NULL,
    vrf_key_hash public.hash32type NOT NULL,
    pledge public.lovelace NOT NULL,
    active_epoch_no bigint NOT NULL,
    meta_id bigint,
    margin double precision NOT NULL,
    fixed_cost public.lovelace NOT NULL,
    registered_tx_id bigint NOT NULL,
    reward_addr public.addr29type NOT NULL
);


ALTER TABLE public.pool_update OWNER TO nixbld;

--
-- Name: pool_update_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.pool_update_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pool_update_id_seq OWNER TO nixbld;

--
-- Name: pool_update_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.pool_update_id_seq OWNED BY public.pool_update.id;


--
-- Name: pot_transfer; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.pot_transfer (
    id bigint NOT NULL,
    cert_index integer NOT NULL,
    treasury public.int65type NOT NULL,
    reserves public.int65type NOT NULL,
    tx_id bigint NOT NULL
);


ALTER TABLE public.pot_transfer OWNER TO nixbld;

--
-- Name: pot_transfer_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.pot_transfer_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pot_transfer_id_seq OWNER TO nixbld;

--
-- Name: pot_transfer_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.pot_transfer_id_seq OWNED BY public.pot_transfer.id;


--
-- Name: reserve; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.reserve (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    cert_index integer NOT NULL,
    amount public.int65type NOT NULL,
    tx_id bigint NOT NULL
);


ALTER TABLE public.reserve OWNER TO nixbld;

--
-- Name: reserve_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.reserve_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.reserve_id_seq OWNER TO nixbld;

--
-- Name: reserve_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.reserve_id_seq OWNED BY public.reserve.id;


--
-- Name: reward; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.reward (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    amount public.lovelace NOT NULL,
    epoch_no bigint NOT NULL,
    pool_id bigint NOT NULL,
    block_id bigint NOT NULL,
    type public.rewardtype NOT NULL
);


ALTER TABLE public.reward OWNER TO nixbld;

--
-- Name: reward_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.reward_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.reward_id_seq OWNER TO nixbld;

--
-- Name: reward_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.reward_id_seq OWNED BY public.reward.id;


--
-- Name: schema_version; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.schema_version (
    id bigint NOT NULL,
    stage_one bigint NOT NULL,
    stage_two bigint NOT NULL,
    stage_three bigint NOT NULL
);


ALTER TABLE public.schema_version OWNER TO nixbld;

--
-- Name: schema_version_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.schema_version_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.schema_version_id_seq OWNER TO nixbld;

--
-- Name: schema_version_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.schema_version_id_seq OWNED BY public.schema_version.id;


--
-- Name: slot_leader; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.slot_leader (
    id bigint NOT NULL,
    hash public.hash28type NOT NULL,
    pool_hash_id bigint,
    description character varying NOT NULL
);


ALTER TABLE public.slot_leader OWNER TO nixbld;

--
-- Name: slot_leader_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.slot_leader_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.slot_leader_id_seq OWNER TO nixbld;

--
-- Name: slot_leader_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.slot_leader_id_seq OWNED BY public.slot_leader.id;


--
-- Name: stake_address; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.stake_address (
    id bigint NOT NULL,
    hash_raw public.addr29type NOT NULL,
    view character varying NOT NULL,
    registered_tx_id bigint NOT NULL
);


ALTER TABLE public.stake_address OWNER TO nixbld;

--
-- Name: stake_address_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.stake_address_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.stake_address_id_seq OWNER TO nixbld;

--
-- Name: stake_address_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.stake_address_id_seq OWNED BY public.stake_address.id;


--
-- Name: stake_deregistration; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.stake_deregistration (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    cert_index integer NOT NULL,
    tx_id bigint NOT NULL
);


ALTER TABLE public.stake_deregistration OWNER TO nixbld;

--
-- Name: stake_deregistration_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.stake_deregistration_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.stake_deregistration_id_seq OWNER TO nixbld;

--
-- Name: stake_deregistration_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.stake_deregistration_id_seq OWNED BY public.stake_deregistration.id;


--
-- Name: stake_registration; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.stake_registration (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    cert_index integer NOT NULL,
    tx_id bigint NOT NULL
);


ALTER TABLE public.stake_registration OWNER TO nixbld;

--
-- Name: stake_registration_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.stake_registration_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.stake_registration_id_seq OWNER TO nixbld;

--
-- Name: stake_registration_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.stake_registration_id_seq OWNED BY public.stake_registration.id;


--
-- Name: treasury; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.treasury (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    cert_index integer NOT NULL,
    amount public.int65type NOT NULL,
    tx_id bigint NOT NULL
);


ALTER TABLE public.treasury OWNER TO nixbld;

--
-- Name: treasury_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.treasury_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.treasury_id_seq OWNER TO nixbld;

--
-- Name: treasury_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.treasury_id_seq OWNED BY public.treasury.id;


--
-- Name: tx; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.tx (
    id bigint NOT NULL,
    hash public.hash32type NOT NULL,
    block_id bigint NOT NULL,
    block_index public.uinteger NOT NULL,
    out_sum public.lovelace NOT NULL,
    fee public.lovelace NOT NULL,
    deposit bigint NOT NULL,
    size public.uinteger NOT NULL,
    invalid_before public.word64type,
    invalid_hereafter public.word64type
);


ALTER TABLE public.tx OWNER TO nixbld;

--
-- Name: tx_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.tx_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tx_id_seq OWNER TO nixbld;

--
-- Name: tx_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.tx_id_seq OWNED BY public.tx.id;


--
-- Name: tx_in; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.tx_in (
    id bigint NOT NULL,
    tx_in_id bigint NOT NULL,
    tx_out_id bigint NOT NULL,
    tx_out_index public.txindex NOT NULL
);


ALTER TABLE public.tx_in OWNER TO nixbld;

--
-- Name: tx_in_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.tx_in_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tx_in_id_seq OWNER TO nixbld;

--
-- Name: tx_in_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.tx_in_id_seq OWNED BY public.tx_in.id;


--
-- Name: tx_metadata; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.tx_metadata (
    id bigint NOT NULL,
    key public.word64type NOT NULL,
    json jsonb,
    tx_id bigint NOT NULL,
    bytes bytea NOT NULL
);


ALTER TABLE public.tx_metadata OWNER TO nixbld;

--
-- Name: tx_metadata_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.tx_metadata_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tx_metadata_id_seq OWNER TO nixbld;

--
-- Name: tx_metadata_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.tx_metadata_id_seq OWNED BY public.tx_metadata.id;


--
-- Name: tx_out; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.tx_out (
    id bigint NOT NULL,
    tx_id bigint NOT NULL,
    index public.txindex NOT NULL,
    address character varying NOT NULL,
    address_raw bytea NOT NULL,
    payment_cred public.hash28type,
    stake_address_id bigint,
    value public.lovelace NOT NULL
);


ALTER TABLE public.tx_out OWNER TO nixbld;

--
-- Name: tx_out_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.tx_out_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tx_out_id_seq OWNER TO nixbld;

--
-- Name: tx_out_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.tx_out_id_seq OWNED BY public.tx_out.id;


--
-- Name: utxo_byron_view; Type: VIEW; Schema: public; Owner: nixbld
--

CREATE VIEW public.utxo_byron_view AS
 SELECT tx_out.id,
    tx_out.tx_id,
    tx_out.index,
    tx_out.address,
    tx_out.address_raw,
    tx_out.payment_cred,
    tx_out.stake_address_id,
    tx_out.value
   FROM (public.tx_out
     LEFT JOIN public.tx_in ON (((tx_out.tx_id = tx_in.tx_out_id) AND ((tx_out.index)::smallint = (tx_in.tx_out_index)::smallint))))
  WHERE (tx_in.tx_in_id IS NULL);


ALTER TABLE public.utxo_byron_view OWNER TO nixbld;

--
-- Name: utxo_view; Type: VIEW; Schema: public; Owner: nixbld
--

CREATE VIEW public.utxo_view AS
 SELECT tx_out.id,
    tx_out.tx_id,
    tx_out.index,
    tx_out.address,
    tx_out.address_raw,
    tx_out.payment_cred,
    tx_out.stake_address_id,
    tx_out.value
   FROM (((public.tx_out
     LEFT JOIN public.tx_in ON (((tx_out.tx_id = tx_in.tx_out_id) AND ((tx_out.index)::smallint = (tx_in.tx_out_index)::smallint))))
     LEFT JOIN public.tx ON ((tx.id = tx_out.tx_id)))
     LEFT JOIN public.block ON ((tx.block_id = block.id)))
  WHERE ((tx_in.tx_in_id IS NULL) AND (block.epoch_no IS NULL));


ALTER TABLE public.utxo_view OWNER TO nixbld;

--
-- Name: withdrawal; Type: TABLE; Schema: public; Owner: nixbld
--

CREATE TABLE public.withdrawal (
    id bigint NOT NULL,
    addr_id bigint NOT NULL,
    amount public.lovelace NOT NULL,
    tx_id bigint NOT NULL
);


ALTER TABLE public.withdrawal OWNER TO nixbld;

--
-- Name: withdrawal_id_seq; Type: SEQUENCE; Schema: public; Owner: nixbld
--

CREATE SEQUENCE public.withdrawal_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.withdrawal_id_seq OWNER TO nixbld;

--
-- Name: withdrawal_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: nixbld
--

ALTER SEQUENCE public.withdrawal_id_seq OWNED BY public.withdrawal.id;


--
-- Name: ada_pots id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ada_pots ALTER COLUMN id SET DEFAULT nextval('public.ada_pots_id_seq'::regclass);


--
-- Name: block id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.block ALTER COLUMN id SET DEFAULT nextval('public.block_id_seq'::regclass);


--
-- Name: delegation id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.delegation ALTER COLUMN id SET DEFAULT nextval('public.delegation_id_seq'::regclass);


--
-- Name: epoch id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch ALTER COLUMN id SET DEFAULT nextval('public.epoch_id_seq'::regclass);


--
-- Name: epoch_param id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_param ALTER COLUMN id SET DEFAULT nextval('public.epoch_param_id_seq'::regclass);


--
-- Name: epoch_stake id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_stake ALTER COLUMN id SET DEFAULT nextval('public.epoch_stake_id_seq'::regclass);


--
-- Name: ma_tx_mint id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_mint ALTER COLUMN id SET DEFAULT nextval('public.ma_tx_mint_id_seq'::regclass);


--
-- Name: ma_tx_out id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_out ALTER COLUMN id SET DEFAULT nextval('public.ma_tx_out_id_seq'::regclass);


--
-- Name: meta id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.meta ALTER COLUMN id SET DEFAULT nextval('public.meta_id_seq'::regclass);


--
-- Name: orphaned_reward id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.orphaned_reward ALTER COLUMN id SET DEFAULT nextval('public.orphaned_reward_id_seq'::regclass);


--
-- Name: param_proposal id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.param_proposal ALTER COLUMN id SET DEFAULT nextval('public.param_proposal_id_seq'::regclass);


--
-- Name: pool_hash id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_hash ALTER COLUMN id SET DEFAULT nextval('public.pool_hash_id_seq'::regclass);


--
-- Name: pool_meta_data id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_meta_data ALTER COLUMN id SET DEFAULT nextval('public.pool_meta_data_id_seq'::regclass);


--
-- Name: pool_owner id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_owner ALTER COLUMN id SET DEFAULT nextval('public.pool_owner_id_seq'::regclass);


--
-- Name: pool_relay id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_relay ALTER COLUMN id SET DEFAULT nextval('public.pool_relay_id_seq'::regclass);


--
-- Name: pool_retire id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_retire ALTER COLUMN id SET DEFAULT nextval('public.pool_retire_id_seq'::regclass);


--
-- Name: pool_update id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_update ALTER COLUMN id SET DEFAULT nextval('public.pool_update_id_seq'::regclass);


--
-- Name: pot_transfer id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pot_transfer ALTER COLUMN id SET DEFAULT nextval('public.pot_transfer_id_seq'::regclass);


--
-- Name: reserve id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reserve ALTER COLUMN id SET DEFAULT nextval('public.reserve_id_seq'::regclass);


--
-- Name: reward id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reward ALTER COLUMN id SET DEFAULT nextval('public.reward_id_seq'::regclass);


--
-- Name: schema_version id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.schema_version ALTER COLUMN id SET DEFAULT nextval('public.schema_version_id_seq'::regclass);


--
-- Name: slot_leader id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.slot_leader ALTER COLUMN id SET DEFAULT nextval('public.slot_leader_id_seq'::regclass);


--
-- Name: stake_address id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_address ALTER COLUMN id SET DEFAULT nextval('public.stake_address_id_seq'::regclass);


--
-- Name: stake_deregistration id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_deregistration ALTER COLUMN id SET DEFAULT nextval('public.stake_deregistration_id_seq'::regclass);


--
-- Name: stake_registration id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_registration ALTER COLUMN id SET DEFAULT nextval('public.stake_registration_id_seq'::regclass);


--
-- Name: treasury id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.treasury ALTER COLUMN id SET DEFAULT nextval('public.treasury_id_seq'::regclass);


--
-- Name: tx id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx ALTER COLUMN id SET DEFAULT nextval('public.tx_id_seq'::regclass);


--
-- Name: tx_in id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_in ALTER COLUMN id SET DEFAULT nextval('public.tx_in_id_seq'::regclass);


--
-- Name: tx_metadata id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_metadata ALTER COLUMN id SET DEFAULT nextval('public.tx_metadata_id_seq'::regclass);


--
-- Name: tx_out id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_out ALTER COLUMN id SET DEFAULT nextval('public.tx_out_id_seq'::regclass);


--
-- Name: withdrawal id; Type: DEFAULT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.withdrawal ALTER COLUMN id SET DEFAULT nextval('public.withdrawal_id_seq'::regclass);


--
-- Name: ada_pots ada_pots_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ada_pots
    ADD CONSTRAINT ada_pots_pkey PRIMARY KEY (id);


--
-- Name: block block_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.block
    ADD CONSTRAINT block_pkey PRIMARY KEY (id);


--
-- Name: delegation delegation_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.delegation
    ADD CONSTRAINT delegation_pkey PRIMARY KEY (id);


--
-- Name: epoch_param epoch_param_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_param
    ADD CONSTRAINT epoch_param_pkey PRIMARY KEY (id);


--
-- Name: epoch epoch_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch
    ADD CONSTRAINT epoch_pkey PRIMARY KEY (id);


--
-- Name: epoch_stake epoch_stake_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_stake
    ADD CONSTRAINT epoch_stake_pkey PRIMARY KEY (id);


--
-- Name: ma_tx_mint ma_tx_mint_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_mint
    ADD CONSTRAINT ma_tx_mint_pkey PRIMARY KEY (id);


--
-- Name: ma_tx_out ma_tx_out_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_out
    ADD CONSTRAINT ma_tx_out_pkey PRIMARY KEY (id);


--
-- Name: meta meta_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.meta
    ADD CONSTRAINT meta_pkey PRIMARY KEY (id);


--
-- Name: orphaned_reward orphaned_reward_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.orphaned_reward
    ADD CONSTRAINT orphaned_reward_pkey PRIMARY KEY (id);


--
-- Name: param_proposal param_proposal_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.param_proposal
    ADD CONSTRAINT param_proposal_pkey PRIMARY KEY (id);


--
-- Name: pool_hash pool_hash_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_hash
    ADD CONSTRAINT pool_hash_pkey PRIMARY KEY (id);


--
-- Name: pool_meta_data pool_meta_data_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_meta_data
    ADD CONSTRAINT pool_meta_data_pkey PRIMARY KEY (id);


--
-- Name: pool_owner pool_owner_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_owner
    ADD CONSTRAINT pool_owner_pkey PRIMARY KEY (id);


--
-- Name: pool_relay pool_relay_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_relay
    ADD CONSTRAINT pool_relay_pkey PRIMARY KEY (id);


--
-- Name: pool_retire pool_retire_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_retire
    ADD CONSTRAINT pool_retire_pkey PRIMARY KEY (id);


--
-- Name: pool_update pool_update_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_update
    ADD CONSTRAINT pool_update_pkey PRIMARY KEY (id);


--
-- Name: pot_transfer pot_transfer_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pot_transfer
    ADD CONSTRAINT pot_transfer_pkey PRIMARY KEY (id);


--
-- Name: reserve reserve_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reserve
    ADD CONSTRAINT reserve_pkey PRIMARY KEY (id);


--
-- Name: reward reward_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reward
    ADD CONSTRAINT reward_pkey PRIMARY KEY (id);


--
-- Name: schema_version schema_version_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.schema_version
    ADD CONSTRAINT schema_version_pkey PRIMARY KEY (id);


--
-- Name: slot_leader slot_leader_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.slot_leader
    ADD CONSTRAINT slot_leader_pkey PRIMARY KEY (id);


--
-- Name: stake_address stake_address_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_address
    ADD CONSTRAINT stake_address_pkey PRIMARY KEY (id);


--
-- Name: stake_deregistration stake_deregistration_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_deregistration
    ADD CONSTRAINT stake_deregistration_pkey PRIMARY KEY (id);


--
-- Name: stake_registration stake_registration_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_registration
    ADD CONSTRAINT stake_registration_pkey PRIMARY KEY (id);


--
-- Name: treasury treasury_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.treasury
    ADD CONSTRAINT treasury_pkey PRIMARY KEY (id);


--
-- Name: tx_in tx_in_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_in
    ADD CONSTRAINT tx_in_pkey PRIMARY KEY (id);


--
-- Name: tx_metadata tx_metadata_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_metadata
    ADD CONSTRAINT tx_metadata_pkey PRIMARY KEY (id);


--
-- Name: tx_out tx_out_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_out
    ADD CONSTRAINT tx_out_pkey PRIMARY KEY (id);


--
-- Name: tx tx_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx
    ADD CONSTRAINT tx_pkey PRIMARY KEY (id);


--
-- Name: ada_pots unique_ada_pots; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ada_pots
    ADD CONSTRAINT unique_ada_pots UNIQUE (block_id);


--
-- Name: block unique_block; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.block
    ADD CONSTRAINT unique_block UNIQUE (hash);


--
-- Name: delegation unique_delegation; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.delegation
    ADD CONSTRAINT unique_delegation UNIQUE (addr_id, pool_hash_id, tx_id);


--
-- Name: epoch unique_epoch; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch
    ADD CONSTRAINT unique_epoch UNIQUE (no);


--
-- Name: epoch_param unique_epoch_param; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_param
    ADD CONSTRAINT unique_epoch_param UNIQUE (epoch_no, block_id);


--
-- Name: ma_tx_mint unique_ma_tx_mint; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_mint
    ADD CONSTRAINT unique_ma_tx_mint UNIQUE (policy, name, tx_id);


--
-- Name: ma_tx_out unique_ma_tx_out; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_out
    ADD CONSTRAINT unique_ma_tx_out UNIQUE (policy, name, tx_out_id);


--
-- Name: meta unique_meta; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.meta
    ADD CONSTRAINT unique_meta UNIQUE (start_time);


--
-- Name: orphaned_reward unique_orphaned; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.orphaned_reward
    ADD CONSTRAINT unique_orphaned UNIQUE (addr_id, block_id);


--
-- Name: param_proposal unique_param_proposal; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.param_proposal
    ADD CONSTRAINT unique_param_proposal UNIQUE (key, registered_tx_id);


--
-- Name: pool_hash unique_pool_hash; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_hash
    ADD CONSTRAINT unique_pool_hash UNIQUE (hash_raw);


--
-- Name: pool_meta_data unique_pool_meta_data; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_meta_data
    ADD CONSTRAINT unique_pool_meta_data UNIQUE (url, hash);


--
-- Name: pool_owner unique_pool_owner; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_owner
    ADD CONSTRAINT unique_pool_owner UNIQUE (addr_id, pool_hash_id, registered_tx_id);


--
-- Name: pool_relay unique_pool_relay; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_relay
    ADD CONSTRAINT unique_pool_relay UNIQUE (update_id, ipv4, ipv6, dns_name);


--
-- Name: pool_retire unique_pool_retiring; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_retire
    ADD CONSTRAINT unique_pool_retiring UNIQUE (hash_id, announced_tx_id);


--
-- Name: pool_update unique_pool_update; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_update
    ADD CONSTRAINT unique_pool_update UNIQUE (hash_id, registered_tx_id);


--
-- Name: pot_transfer unique_pot_transfer; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pot_transfer
    ADD CONSTRAINT unique_pot_transfer UNIQUE (tx_id, cert_index);


--
-- Name: reserve unique_reserves; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reserve
    ADD CONSTRAINT unique_reserves UNIQUE (addr_id, tx_id);


--
-- Name: reward unique_reward; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reward
    ADD CONSTRAINT unique_reward UNIQUE (addr_id, block_id);


--
-- Name: slot_leader unique_slot_leader; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.slot_leader
    ADD CONSTRAINT unique_slot_leader UNIQUE (hash);


--
-- Name: epoch_stake unique_stake; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_stake
    ADD CONSTRAINT unique_stake UNIQUE (addr_id, epoch_no);


--
-- Name: stake_address unique_stake_address; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_address
    ADD CONSTRAINT unique_stake_address UNIQUE (hash_raw);


--
-- Name: stake_deregistration unique_stake_deregistration; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_deregistration
    ADD CONSTRAINT unique_stake_deregistration UNIQUE (addr_id, tx_id);


--
-- Name: stake_registration unique_stake_registration; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_registration
    ADD CONSTRAINT unique_stake_registration UNIQUE (addr_id, tx_id);


--
-- Name: treasury unique_treasury; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.treasury
    ADD CONSTRAINT unique_treasury UNIQUE (addr_id, tx_id);


--
-- Name: tx unique_tx; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx
    ADD CONSTRAINT unique_tx UNIQUE (hash);


--
-- Name: tx_metadata unique_tx_metadata; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_metadata
    ADD CONSTRAINT unique_tx_metadata UNIQUE (key, tx_id);


--
-- Name: tx_in unique_txin; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_in
    ADD CONSTRAINT unique_txin UNIQUE (tx_out_id, tx_out_index);


--
-- Name: tx_out unique_txout; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_out
    ADD CONSTRAINT unique_txout UNIQUE (tx_id, index);


--
-- Name: withdrawal unique_withdrawal; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.withdrawal
    ADD CONSTRAINT unique_withdrawal UNIQUE (addr_id, tx_id);


--
-- Name: withdrawal withdrawal_pkey; Type: CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.withdrawal
    ADD CONSTRAINT withdrawal_pkey PRIMARY KEY (id);


--
-- Name: idx_block_block_no; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_block_block_no ON public.block USING btree (block_no);


--
-- Name: idx_block_epoch_no; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_block_epoch_no ON public.block USING btree (epoch_no);


--
-- Name: idx_block_previous_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_block_previous_id ON public.block USING btree (previous_id);


--
-- Name: idx_block_slot_leader_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_block_slot_leader_id ON public.block USING btree (slot_leader_id);


--
-- Name: idx_block_slot_no; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_block_slot_no ON public.block USING btree (slot_no);


--
-- Name: idx_block_time; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_block_time ON public.block USING btree ("time");


--
-- Name: idx_delegation_active_epoch_no; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_delegation_active_epoch_no ON public.delegation USING btree (active_epoch_no);


--
-- Name: idx_delegation_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_delegation_addr_id ON public.delegation USING btree (addr_id);


--
-- Name: idx_delegation_pool_hash_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_delegation_pool_hash_id ON public.delegation USING btree (pool_hash_id);


--
-- Name: idx_delegation_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_delegation_tx_id ON public.delegation USING btree (tx_id);


--
-- Name: idx_epoch_param_block_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_epoch_param_block_id ON public.epoch_param USING btree (block_id);


--
-- Name: idx_epoch_stake_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_epoch_stake_addr_id ON public.epoch_stake USING btree (addr_id);


--
-- Name: idx_epoch_stake_block_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_epoch_stake_block_id ON public.epoch_stake USING btree (block_id);


--
-- Name: idx_epoch_stake_pool_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_epoch_stake_pool_id ON public.epoch_stake USING btree (pool_id);


--
-- Name: idx_ma_tx_mint_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_ma_tx_mint_tx_id ON public.ma_tx_mint USING btree (tx_id);


--
-- Name: idx_ma_tx_out_tx_out_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_ma_tx_out_tx_out_id ON public.ma_tx_out USING btree (tx_out_id);


--
-- Name: idx_orphaned_reward_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_orphaned_reward_addr_id ON public.orphaned_reward USING btree (addr_id);


--
-- Name: idx_orphaned_reward_block_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_orphaned_reward_block_id ON public.orphaned_reward USING btree (block_id);


--
-- Name: idx_orphaned_reward_pool_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_orphaned_reward_pool_id ON public.orphaned_reward USING btree (pool_id);


--
-- Name: idx_param_proposal_registered_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_param_proposal_registered_tx_id ON public.param_proposal USING btree (registered_tx_id);


--
-- Name: idx_pool_meta_data_registered_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_meta_data_registered_tx_id ON public.pool_meta_data USING btree (registered_tx_id);


--
-- Name: idx_pool_owner_pool_hash_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_owner_pool_hash_id ON public.pool_owner USING btree (pool_hash_id);


--
-- Name: idx_pool_owner_registered_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_owner_registered_tx_id ON public.pool_owner USING btree (registered_tx_id);


--
-- Name: idx_pool_relay_update_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_relay_update_id ON public.pool_relay USING btree (update_id);


--
-- Name: idx_pool_retire_announced_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_retire_announced_tx_id ON public.pool_retire USING btree (announced_tx_id);


--
-- Name: idx_pool_retire_hash_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_retire_hash_id ON public.pool_retire USING btree (hash_id);


--
-- Name: idx_pool_update_active_epoch_no; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_update_active_epoch_no ON public.pool_update USING btree (active_epoch_no);


--
-- Name: idx_pool_update_hash_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_update_hash_id ON public.pool_update USING btree (hash_id);


--
-- Name: idx_pool_update_meta_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_update_meta_id ON public.pool_update USING btree (meta_id);


--
-- Name: idx_pool_update_registered_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_update_registered_tx_id ON public.pool_update USING btree (registered_tx_id);


--
-- Name: idx_pool_update_reward_addr; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_pool_update_reward_addr ON public.pool_update USING btree (reward_addr);


--
-- Name: idx_reserve_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_reserve_addr_id ON public.reserve USING btree (addr_id);


--
-- Name: idx_reserve_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_reserve_tx_id ON public.reserve USING btree (tx_id);


--
-- Name: idx_reward_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_reward_addr_id ON public.reward USING btree (addr_id);


--
-- Name: idx_reward_block_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_reward_block_id ON public.reward USING btree (block_id);


--
-- Name: idx_reward_pool_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_reward_pool_id ON public.reward USING btree (pool_id);


--
-- Name: idx_slot_leader_pool_hash_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_slot_leader_pool_hash_id ON public.slot_leader USING btree (pool_hash_id);


--
-- Name: idx_stake_address_hash_raw; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_stake_address_hash_raw ON public.stake_address USING btree (hash_raw);


--
-- Name: idx_stake_address_registered_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_stake_address_registered_tx_id ON public.stake_address USING btree (registered_tx_id);


--
-- Name: idx_stake_deregistration_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_stake_deregistration_addr_id ON public.stake_deregistration USING btree (addr_id);


--
-- Name: idx_stake_deregistration_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_stake_deregistration_tx_id ON public.stake_deregistration USING btree (tx_id);


--
-- Name: idx_stake_registration_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_stake_registration_addr_id ON public.stake_registration USING btree (addr_id);


--
-- Name: idx_stake_registration_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_stake_registration_tx_id ON public.stake_registration USING btree (tx_id);


--
-- Name: idx_treasury_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_treasury_addr_id ON public.treasury USING btree (addr_id);


--
-- Name: idx_treasury_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_treasury_tx_id ON public.treasury USING btree (tx_id);


--
-- Name: idx_tx_block_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_block_id ON public.tx USING btree (block_id);


--
-- Name: idx_tx_in_source_tx; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_in_source_tx ON public.tx_in USING btree (tx_in_id);


--
-- Name: idx_tx_in_tx_in_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_in_tx_in_id ON public.tx_in USING btree (tx_in_id);


--
-- Name: idx_tx_in_tx_out_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_in_tx_out_id ON public.tx_in USING btree (tx_out_id);


--
-- Name: idx_tx_metadata_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_metadata_tx_id ON public.tx_metadata USING btree (tx_id);


--
-- Name: idx_tx_out_address; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_out_address ON public.tx_out USING hash (address);


--
-- Name: idx_tx_out_payment_cred; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_out_payment_cred ON public.tx_out USING btree (payment_cred);


--
-- Name: idx_tx_out_stake_address_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_out_stake_address_id ON public.tx_out USING btree (stake_address_id);


--
-- Name: idx_tx_out_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_tx_out_tx_id ON public.tx_out USING btree (tx_id);


--
-- Name: idx_withdrawal_addr_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_withdrawal_addr_id ON public.withdrawal USING btree (addr_id);


--
-- Name: idx_withdrawal_tx_id; Type: INDEX; Schema: public; Owner: nixbld
--

CREATE INDEX idx_withdrawal_tx_id ON public.withdrawal USING btree (tx_id);


--
-- Name: ada_pots ada_pots_block_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ada_pots
    ADD CONSTRAINT ada_pots_block_id_fkey FOREIGN KEY (block_id) REFERENCES public.block(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: block block_previous_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.block
    ADD CONSTRAINT block_previous_id_fkey FOREIGN KEY (previous_id) REFERENCES public.block(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: block block_slot_leader_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.block
    ADD CONSTRAINT block_slot_leader_id_fkey FOREIGN KEY (slot_leader_id) REFERENCES public.slot_leader(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: delegation delegation_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.delegation
    ADD CONSTRAINT delegation_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: delegation delegation_pool_hash_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.delegation
    ADD CONSTRAINT delegation_pool_hash_id_fkey FOREIGN KEY (pool_hash_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: delegation delegation_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.delegation
    ADD CONSTRAINT delegation_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: epoch_param epoch_param_block_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_param
    ADD CONSTRAINT epoch_param_block_id_fkey FOREIGN KEY (block_id) REFERENCES public.block(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: epoch_stake epoch_stake_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_stake
    ADD CONSTRAINT epoch_stake_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: epoch_stake epoch_stake_block_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_stake
    ADD CONSTRAINT epoch_stake_block_id_fkey FOREIGN KEY (block_id) REFERENCES public.block(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: epoch_stake epoch_stake_pool_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.epoch_stake
    ADD CONSTRAINT epoch_stake_pool_id_fkey FOREIGN KEY (pool_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: ma_tx_mint ma_tx_mint_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_mint
    ADD CONSTRAINT ma_tx_mint_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: ma_tx_out ma_tx_out_tx_out_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.ma_tx_out
    ADD CONSTRAINT ma_tx_out_tx_out_id_fkey FOREIGN KEY (tx_out_id) REFERENCES public.tx_out(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: orphaned_reward orphaned_reward_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.orphaned_reward
    ADD CONSTRAINT orphaned_reward_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: orphaned_reward orphaned_reward_block_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.orphaned_reward
    ADD CONSTRAINT orphaned_reward_block_id_fkey FOREIGN KEY (block_id) REFERENCES public.block(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: orphaned_reward orphaned_reward_pool_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.orphaned_reward
    ADD CONSTRAINT orphaned_reward_pool_id_fkey FOREIGN KEY (pool_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: param_proposal param_proposal_registered_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.param_proposal
    ADD CONSTRAINT param_proposal_registered_tx_id_fkey FOREIGN KEY (registered_tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_meta_data pool_meta_data_registered_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_meta_data
    ADD CONSTRAINT pool_meta_data_registered_tx_id_fkey FOREIGN KEY (registered_tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_owner pool_owner_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_owner
    ADD CONSTRAINT pool_owner_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_owner pool_owner_pool_hash_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_owner
    ADD CONSTRAINT pool_owner_pool_hash_id_fkey FOREIGN KEY (pool_hash_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_owner pool_owner_registered_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_owner
    ADD CONSTRAINT pool_owner_registered_tx_id_fkey FOREIGN KEY (registered_tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_relay pool_relay_update_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_relay
    ADD CONSTRAINT pool_relay_update_id_fkey FOREIGN KEY (update_id) REFERENCES public.pool_update(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_retire pool_retire_announced_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_retire
    ADD CONSTRAINT pool_retire_announced_tx_id_fkey FOREIGN KEY (announced_tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_retire pool_retire_hash_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_retire
    ADD CONSTRAINT pool_retire_hash_id_fkey FOREIGN KEY (hash_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_update pool_update_hash_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_update
    ADD CONSTRAINT pool_update_hash_id_fkey FOREIGN KEY (hash_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_update pool_update_meta_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_update
    ADD CONSTRAINT pool_update_meta_id_fkey FOREIGN KEY (meta_id) REFERENCES public.pool_meta_data(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pool_update pool_update_registered_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pool_update
    ADD CONSTRAINT pool_update_registered_tx_id_fkey FOREIGN KEY (registered_tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: pot_transfer pot_transfer_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.pot_transfer
    ADD CONSTRAINT pot_transfer_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: reserve reserve_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reserve
    ADD CONSTRAINT reserve_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: reserve reserve_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reserve
    ADD CONSTRAINT reserve_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: reward reward_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reward
    ADD CONSTRAINT reward_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: reward reward_block_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reward
    ADD CONSTRAINT reward_block_id_fkey FOREIGN KEY (block_id) REFERENCES public.block(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: reward reward_pool_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.reward
    ADD CONSTRAINT reward_pool_id_fkey FOREIGN KEY (pool_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: slot_leader slot_leader_pool_hash_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.slot_leader
    ADD CONSTRAINT slot_leader_pool_hash_id_fkey FOREIGN KEY (pool_hash_id) REFERENCES public.pool_hash(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: stake_address stake_address_registered_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_address
    ADD CONSTRAINT stake_address_registered_tx_id_fkey FOREIGN KEY (registered_tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: stake_deregistration stake_deregistration_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_deregistration
    ADD CONSTRAINT stake_deregistration_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: stake_deregistration stake_deregistration_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_deregistration
    ADD CONSTRAINT stake_deregistration_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: stake_registration stake_registration_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_registration
    ADD CONSTRAINT stake_registration_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: stake_registration stake_registration_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.stake_registration
    ADD CONSTRAINT stake_registration_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: treasury treasury_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.treasury
    ADD CONSTRAINT treasury_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: treasury treasury_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.treasury
    ADD CONSTRAINT treasury_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: tx tx_block_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx
    ADD CONSTRAINT tx_block_id_fkey FOREIGN KEY (block_id) REFERENCES public.block(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: tx_in tx_in_tx_in_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_in
    ADD CONSTRAINT tx_in_tx_in_id_fkey FOREIGN KEY (tx_in_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: tx_in tx_in_tx_out_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_in
    ADD CONSTRAINT tx_in_tx_out_id_fkey FOREIGN KEY (tx_out_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: tx_metadata tx_metadata_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_metadata
    ADD CONSTRAINT tx_metadata_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: tx_out tx_out_stake_address_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_out
    ADD CONSTRAINT tx_out_stake_address_id_fkey FOREIGN KEY (stake_address_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: tx_out tx_out_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.tx_out
    ADD CONSTRAINT tx_out_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: withdrawal withdrawal_addr_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.withdrawal
    ADD CONSTRAINT withdrawal_addr_id_fkey FOREIGN KEY (addr_id) REFERENCES public.stake_address(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: withdrawal withdrawal_tx_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: nixbld
--

ALTER TABLE ONLY public.withdrawal
    ADD CONSTRAINT withdrawal_tx_id_fkey FOREIGN KEY (tx_id) REFERENCES public.tx(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: nixbld
--

REVOKE ALL ON SCHEMA public FROM postgres;
REVOKE ALL ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO nixbld;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

